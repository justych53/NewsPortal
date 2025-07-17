using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Core.Models;

namespace NewsPortal.DataAccess.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name)
                .HasMaxLength(Category.MAX_LENGTH)
                .IsRequired();
        }
    }
}
