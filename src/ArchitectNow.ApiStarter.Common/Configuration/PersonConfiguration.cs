using ArchitectNow.ApiStarter.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchitectNow.ApiStarter.Common.Configuration
{
    public class PersonConfiguration : BaseEntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);
        }
    }
}