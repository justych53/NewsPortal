using Microsoft.EntityFrameworkCore;
using NewsPortal.DataAccess.Entities;

namespace NewsPortal.DataAccess
{
    public class NewsPortalDbContext : DbContext
    {
        public NewsPortalDbContext(DbContextOptions<NewsPortalDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<NewsEntity> News { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
    }
}
