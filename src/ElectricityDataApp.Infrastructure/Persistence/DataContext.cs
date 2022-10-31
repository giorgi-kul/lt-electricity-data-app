using ElectricityDataApp.Application.Interfaces;
using ElectricityDataApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectricityDataApp.Infrastructure.Persistence
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<DataItem> DataItems { get; set; }
    }
}
