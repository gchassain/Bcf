using Bcf.Interfaces;
using Bcf.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcf.Data
{
    public class EFPlayerRepository : IPlayerRepository
    {
        private readonly BcfContext _dbContext;

        public EFPlayerRepository(BcfContext bcfContext)
        {
            _dbContext = bcfContext;
        }

        public Task AddAsync(Player player)
        {
            _dbContext.Players
                .Add(player);
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(Player player)
        {
            _dbContext.Players
                .Remove(player);
            return _dbContext.SaveChangesAsync();
        }

        public bool Exist(int id)
        {
            return _dbContext.Players
                .Any(e => e.Id == id);
        }

        public Task<Player> GetByIdAsync(int id)
        {
            return _dbContext.Players
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<List<Player>> ListAsync(string searchString)
        {
            return _dbContext.Players
                .Where(p => p.LastName.Contains(searchString) || p.FirstName.Contains(searchString))
                .OrderBy(p => p.FirstName)
                .ToListAsync();
        }

        public Task UpdateAsync(Player player)
        {
            _dbContext.Entry(player).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }
    }
}
