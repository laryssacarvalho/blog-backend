using BlogApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlogApi.Infrastructure.Database
{
    public class BlogDbContext : DbContext 
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
