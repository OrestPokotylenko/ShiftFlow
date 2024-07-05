using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DAL
{
    internal class DeepLinkConfiguration : IEntityTypeConfiguration<DeepLink>
    {
        public void Configure(EntityTypeBuilder<DeepLink> builder)
        {
            builder.HasKey(dl => dl.DeepLinkId);
            builder.Property(dl => dl.DeepLinkId).ValueGeneratedOnAdd();
            builder.Property(dl => dl.Link).IsRequired().HasMaxLength(100);
            builder.Property(dl => dl.ExpirationDate).IsRequired();

            builder.HasOne(dl => dl.Employee)
                .WithMany()
                .HasForeignKey(dl => dl.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
