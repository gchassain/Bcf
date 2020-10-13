using Bcf.Models;
using Microsoft.EntityFrameworkCore;

namespace Bcf.Data
{
    public class BcfContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

        public BcfContext(DbContextOptions<BcfContext> options)
            : base(options)
        { }
    }
}
