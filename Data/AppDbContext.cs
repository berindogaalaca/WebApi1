using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi1.Models;

namespace WebApi1.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Value> Values { get; set; }
        protected readonly IConfiguration Configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }
    }
}
