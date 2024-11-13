using Mediagram.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mediagram.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<SubArticle> SubArticles { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
