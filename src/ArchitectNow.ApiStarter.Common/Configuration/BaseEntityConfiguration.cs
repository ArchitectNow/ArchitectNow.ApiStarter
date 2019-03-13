using ArchitectNow.ApiStarter.Common.BaseDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchitectNow.ApiStarter.Common.Configuration
{
    public class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseDocument
    {
        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.Ignore(b => b.ValidationErrors);
            builder.Ignore(b => b.ExtraElements);
        }
    }
}