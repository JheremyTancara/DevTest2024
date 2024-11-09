using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Options.Models;
using Project.Core.Entities.Polls.Models;
using Project.Core.Entities.Products.Models;

namespace Project.DataAccess
{
    public class AppDataContext : DbContext
    {
        public DbSet<Vote> Votes => Set<Vote>();
        public DbSet<Poll> Polls => Set<Poll>();
        public DbSet<PollOption> PollOptions => Set<PollOption>();
        public DbSet<Product> Products => Set<Product>();
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
        }
    }
}