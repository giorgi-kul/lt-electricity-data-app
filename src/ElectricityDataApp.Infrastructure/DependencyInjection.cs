using ElectricityDataApp.Application.Interfaces;
using ElectricityDataApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElectricityDataApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IDataContext>(provider => provider.GetRequiredService<DataContext>());

            return services;
        }
    }
}
