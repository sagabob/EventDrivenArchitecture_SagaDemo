using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Newsletter.Api.Features.Newsletters.Sagas;

public class NewsletterOnboardingSagaMap : IEntityTypeConfiguration<NewsletterOnboardingSagaData>
{
    public void Configure(EntityTypeBuilder<NewsletterOnboardingSagaData> entity)
    {
        entity.ToTable("NewsletterOnboardingSaga");
        entity.HasKey(x => x.CorrelationId);
        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.Email).HasMaxLength(320);
        entity.Property(x => x.ErrorMessage).HasMaxLength(2000);
        entity.Property(x => x.RowVersion).IsRowVersion();
    }
}