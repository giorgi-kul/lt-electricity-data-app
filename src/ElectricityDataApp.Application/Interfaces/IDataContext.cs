using ElectricityDataApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectricityDataApp.Application.Interfaces
{
    public interface IDataContext
    {
        DbSet<Region> Regions { get; set; }

        DbSet<DataItem> DataItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
