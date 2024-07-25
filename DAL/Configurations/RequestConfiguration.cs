using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DAL.Configurations
{
    internal class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(r => r.RequestId);
            builder.Property(r => r.RequestId).ValueGeneratedOnAdd();
            builder.Property(r => r.EmployeeId).IsRequired();
            builder.Property(r => r.StartDate).IsRequired(false);
            builder.Property(r => r.EndDate).IsRequired(false); 
            builder.Property(r => r.RequestDate).IsRequired();
            builder.Property(r => r.Note).IsRequired(false).HasMaxLength(100);
            builder.Property(r => r.Approved).IsRequired(false);

            builder.Property(r => r.RequestType)
                .HasConversion(r => r.ToString(), r => (RequestType)Enum.Parse(typeof(RequestType), r))
                .IsRequired().HasMaxLength(20);

            builder.HasOne(r => r.Employee)
                .WithMany()
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}