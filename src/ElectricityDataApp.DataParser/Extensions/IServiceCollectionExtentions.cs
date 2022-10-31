using ElectricityDataApp.DataParser;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ASP.NET Core service collection extentions for service configuration
    /// </summary>
    public static class IServiceCollectionExtentions
    {
        /// <summary>
        /// Add services required for iPay
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="settingsKey"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataParser(this IServiceCollection services, IConfiguration configuration, string settingsKey)
        {
            ConfigureServices(services);

            services.Configure<DataParserClientOptions>(configuration.GetSection(settingsKey));

            return services;
        }

        /// <summary>
        /// Add services required for iPay
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataParser(this IServiceCollection services, Action<DataParserClientOptions> options)
        {
            ConfigureServices(services);

            services.Configure(options);

            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataParserClient, DataParserClient>();
            services.AddHttpClient<DataParserClient, DataParserClient>();
        }
    }
}