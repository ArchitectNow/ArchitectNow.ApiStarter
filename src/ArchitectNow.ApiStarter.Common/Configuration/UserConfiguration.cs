using ArchitectNow.ApiStarter.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchitectNow.ApiStarter.Common.Configuration
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
        }
    }
}