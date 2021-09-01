namespace PaymentGateway.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
    using PaymentGateway.Api.Swagger;
    using Query;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.Swagger;

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
                 .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

            ConfigureSwagger(services);

        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PaymentGateway.Api",
                    Description =
                        "Naive Payment Gateway API with stubbed Bank simulator",
                    Contact = new OpenApiContact
                    {
                        Name = "Colin Dooley",
                        Email = "colin_dooley@yahoo.co.uk",
                        Url = new Uri("https://www.linkedin.com/in/colin-dooley-87106871/")
                    }
                });

                options.AddFluentValidationRules();
                var apiKeySecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "APIKey",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "ApiKey must appear in header!",
                    Reference = new OpenApiReference { Id = "APIKey", Type = ReferenceType.SecurityScheme }

                };

                var filePath = Path.Combine(AppContext.BaseDirectory, "PaymentGateway.Api.Models.xml"); 
                options.IncludeXmlComments(filePath);

                options.AddSecurityDefinition(apiKeySecurityScheme.Reference.Id, apiKeySecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {apiKeySecurityScheme, new string[] { }}
                });
                options.AddServer(new OpenApiServer { Url = "/", Description = "Local Development Server" });
            });
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