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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);
            try
            {
                foreach (var entry in insertedEntries)
                {
                    if (entry is BaseEntity)
                    {
                        var newEntry = entry as BaseEntity;

                        newEntry.CreateDate = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
