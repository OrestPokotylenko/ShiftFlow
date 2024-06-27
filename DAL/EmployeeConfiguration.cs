using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DAL
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.BirthDate).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(e => e.Occupation).IsRequired().HasMaxLength(25);
            builder.Property(e => e.EmployeeNumber).IsRequired().HasMaxLength(8);
            builder.HasIndex(e => e.EmployeeNumber).IsUnique();
            
            builder.Property<string>("_password").HasColumnName("Password").IsRequired().HasMaxLength(64);
            builder.Property<byte[]>("_salt").HasColumnName("Salt").IsRequired().HasMaxLength(16);
        }
    }
}
