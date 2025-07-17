using NewsPortal.Core.Models;
using NewsPortal.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.DataAccess.Configurations
{
    internal class NewsConfiguration : IEntityTypeConfiguration<NewsEntity>
    {
        public void Configure(EntityTypeBuilder<NewsEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
                .HasMaxLength(News.MAX_LENGTH)
                .IsRequired();
            builder.HasOne<CategoryEntity>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.ShortPhrase)
                .HasMaxLength(News.MAX_LENGTH)
                .IsRequired();
            builder.Property(x => x.Description)
                .IsRequired();

        }
    }
}
