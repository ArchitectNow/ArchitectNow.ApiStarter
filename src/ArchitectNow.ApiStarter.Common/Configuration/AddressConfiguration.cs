using ArchitectNow.ApiStarter.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchitectNow.ApiStarter.Common.Configuration
{
    public class AddressConfiguration : BaseEntityConfiguration<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne(p => p.Person).WithMany(p => p.Addresses)
                .HasForeignKey(p => p.PersonId);
            base.Configure(builder);
        }
    }
}