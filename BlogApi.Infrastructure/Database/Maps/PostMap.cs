using BlogApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Database.Maps
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Content)
                .IsRequired();

            builder.Property(p => p.PublishedAt)
                .IsRequired(false);

            //builder.HasMany(p => p.Comments)
            //    .WithOne(p => p.Post)
            //    .HasForeignKey(p => p.PostId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Author);
        }
    }
}
