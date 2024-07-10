using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DAL.Configurations
{
    internal class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.HasKey(s => s.ShiftId);
            builder.Property(s => s.ShiftId).ValueGeneratedOnAdd();
            builder.Property(s => s.StartTime).IsRequired();
            builder.Property(s => s.EndTime).IsRequired();
            builder.Property(s => s.DepartmentType)
                .HasConversion(d => d.ToString(), d => (DepartmentType)Enum.Parse(typeof(DepartmentType), d))
                .IsRequired().HasMaxLength(20);

            builder.HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
