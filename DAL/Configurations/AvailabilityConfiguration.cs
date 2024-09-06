using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DAL.Configurations
{
    internal class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.HasKey(a => a.AvailabilityId);
            builder.Property(a => a.AvailabilityId).ValueGeneratedOnAdd();
            builder.Property(a => a.EmployeeId).IsRequired();
            builder.Property(a => a.DayOfWeek).IsRequired();

            builder.HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}