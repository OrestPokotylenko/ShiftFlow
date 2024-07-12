using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DAL.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId);
            builder.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.BirthDate).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(e => e.HireDate).IsRequired();
            builder.Property(e => e.EmployeeNumber).IsRequired().HasMaxLength(8);
            builder.HasIndex(e => e.EmployeeNumber).IsUnique();

            builder.Property(e => e.Occupation)
                .HasConversion(o => o.ToString(), o => (OccupationType)Enum.Parse(typeof(OccupationType), o))
                    .IsRequired().HasMaxLength(25);

            builder.Property<string>("_password").HasColumnName("Password").IsRequired().HasMaxLength(64);
            builder.Property<byte[]>("_salt").HasColumnName("Salt").IsRequired().HasMaxLength(16);
        }
    }
}
