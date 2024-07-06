using Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DAL.Configurations;

namespace DAL
{
    public class ShiftFlowContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<DeepLink> DeepLinks { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ShiftFlowDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DeepLinkConfiguration());
            modelBuilder.ApplyConfiguration(new ShiftConfiguration());
        }
    }
}
