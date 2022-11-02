using ElectricityDataApp.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Infrastructure.Persistence
{
    public class DataContextInitializer
    {
        private readonly DataContext _context;

        public DataContextInitializer(DataContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
    }
}
