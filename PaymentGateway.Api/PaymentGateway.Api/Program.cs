namespace PaymentGateway.Api
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.ApplicationInsights;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("From Program, running the host now.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureLogging((context, builder) =>
                        {

                            builder.AddApplicationInsights(
                                context.Configuration["AppInsightsKey"]);

                            builder.AddFilter<ApplicationInsightsLoggerProvider>(
                                typeof(Program).FullName, LogLevel.Trace);


                            builder.AddFilter<ApplicationInsightsLoggerProvider>(
                                typeof(Startup).FullName, LogLevel.Trace);
                        });
                });
    }
}
