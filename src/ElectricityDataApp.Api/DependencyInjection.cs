using ElectricityDataApp.Api.Services;
using Hangfire;
using Hangfire.Logging;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace ElectricityDataApp.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config, ConfigureHostBuilder host)
        {
            string connectionString = config.GetConnectionString("DbConnection");

            services.AddScoped<IJobService, JobService>();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHangfire(configuration => configuration
             .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
             {
                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(10),
                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(10),
                 QueuePollInterval = TimeSpan.Zero,
                 UseRecommendedIsolationLevel = true,
                 DisableGlobalLocks = true
             })
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings());

            JobStorage.Current = new SqlServerStorage(connectionString);
            services.AddHangfireServer();

            services.AddDataParser(opt => config.GetSection("DataParser").Bind(opt));

            ConfigureRecurringJobs();

            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(config)
              .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            host.UseSerilog();

            return services;
        }


        private static void ConfigureRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IJobService>(jobService => jobService.ProcessElectricityData(), Cron.Monthly());
        }
    }
}
