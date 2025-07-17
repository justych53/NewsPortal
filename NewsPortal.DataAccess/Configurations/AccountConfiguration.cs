using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPortal.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Models;

namespace NewsPortal.DataAccess.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName)
                .HasMaxLength(Account.MAX_LENGTH)
                .IsRequired();
            builder.Property(x => x.FirstName)
                .HasMaxLength(Account.MAX_LENGTH)
                .IsRequired();
            builder.Property(x => x.PasswordHash)
                .IsRequired();
        }
    }
}
