using Bcf.Models;
using Microsoft.EntityFrameworkCore;

namespace Bcf.Data
{
    public class BcfContext : DbContext
    {

        public BcfContext(DbContextOptions<BcfContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
