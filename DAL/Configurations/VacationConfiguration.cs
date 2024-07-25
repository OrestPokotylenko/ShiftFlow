using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DAL.Configurations
{
    internal class VacationConfiguration : IEntityTypeConfiguration<Vacation>
    {
        public void Configure(EntityTypeBuilder<Vacation> builder)
        {
            builder.HasKey(v => v.VacationId);
            builder.Property(v => v.VacationId).ValueGeneratedOnAdd();
            builder.Property(v => v.EmployeeId).IsRequired();
            builder.Property(v => v.StartDate).IsRequired();
            builder.Property(v => v.EndDate).IsRequired();
            builder.Property(v => v.Note).HasMaxLength(100);

            builder.HasOne(v => v.Employee)
                .WithMany()
                .HasForeignKey(v => v.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}