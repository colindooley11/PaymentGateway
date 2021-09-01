namespace PaymentGateway.Api
{
    using System.Text.Json.Serialization;
    using BankSimulator;
    using Builders;
    using Clients;
    using Commands;
    using Filters;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Query;

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
            services.AddControllers(options =>
                options.Filters.Add<ValidateModelFilter>()
            ).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentGateway.Api", Version = "v1" });
            });

            this.BuildCosmosFactory(services, Configuration["CosmosAuthEndpoint"], Configuration["CosmosAuthKey"],
                "CardPayments");
            services.AddSingleton<ISaveCardPaymentCommand, SaveCardPaymentCosmosCommand>();
            services.AddSingleton<IGetPaymentDetailsQuery, GetPaymentDetailsCosmosQuery>();

            services.AddHttpClient<AcquiringBankClient>()
                .AddHttpMessageHandler(() => new BankSimulatorStub());

            services.AddMvc().AddFluentValidation(configuration =>
                configuration.RegisterValidatorsFromAssemblyContaining<Program>());

            services.AddAuthorization();

           services.AddAuthentication("ApiKey")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("ApiKey", null);
        }

        protected virtual void BuildCosmosFactory(IServiceCollection services, string accountEndpoint,
            string authKeyOrResourceToken, string cardPaymentsContainer)
        {
            var cosmosBuilderFactory = new CosmosBuilderFactory();
            var container = cosmosBuilderFactory.Build(accountEndpoint, authKeyOrResourceToken, cardPaymentsContainer,
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}