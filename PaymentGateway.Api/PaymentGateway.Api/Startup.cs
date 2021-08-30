namespace PaymentGateway.Api
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Builders;
    using Commands;
    using FluentValidation.AspNetCore;
    using Gateways;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Models;
    using Models.Web;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentGateway.Api", Version = "v1" });
            });

            this.BuildCosmosFactory(services, Configuration["CosmosAuthEndpoint"], Configuration["CosmosAuthKey"],
                "CardPayments");
            services.AddSingleton<IAcquiringBankGateway, AcquiringBankGateway>();
            services.AddSingleton<ISaveCardPaymentCommand, SaveCardPaymentCommand>();
            services.AddMvc().AddFluentValidation(configuration =>
                configuration.RegisterValidatorsFromAssemblyContaining<Program>());
        }

        protected virtual void BuildCosmosFactory(IServiceCollection services, string accountEndpoint,
            string authKeyOrResourceToken, string cardpaymentscontainer)
        {
            var cosmosBuilderFactory = new CosmosBuilderFactory();
            var container = cosmosBuilderFactory.Build(accountEndpoint, authKeyOrResourceToken, cardpaymentscontainer,
                "PaymentGateway.Api");
            services.AddSingleton(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentGateway.Api v1"));
            }

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public class AcquiringBankGateway : IAcquiringBankGateway
    {
        private readonly IHttpClientFactory _clientFactory;

        public AcquiringBankGateway(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<AcquiringBankResponse> CapturePayment(CardPaymentRequest cardPaymentRequest)
        {
            var client = _clientFactory.CreateClient("AcquiringBankGateway");
            return new AcquiringBankResponse();
        }
    }
}