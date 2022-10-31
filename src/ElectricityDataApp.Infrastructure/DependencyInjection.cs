using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricityDataApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElectricityDataApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"),
                    builder => builder.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));


            services.AddScoped<DataContext>(provider => provider.GetRequiredService<DataContext>());

            return services;
        }
    }
}
