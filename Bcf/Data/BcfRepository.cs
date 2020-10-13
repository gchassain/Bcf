using Bcf.Interfaces;
using Bcf.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcf.Data
{
    public class BcfRepository : IBcfRepository
    {
        private readonly BcfContext _dbContext;

        public BcfRepository(BcfContext bcfContext)
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

        public Task<List<Player>> ListPlayersAsync()
        {
            return _dbContext.Players
                .OrderBy(p => p.FirstName)
                .ToListAsync();
        }

        public IQueryable<Player> ListPlayers()
        {
            return _dbContext.Players;
        }

        /*public Task SaveChangeAsync(Player player)
        {
            _dbContext.Players.Add(player);
            return _dbContext.SaveChangesAsync();
        }*/

        public Task UpdateAsync(Player player)
        {
            _dbContext.Entry(player).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task<List<Team>> ListTeamsAsync()
        {
            return _dbContext.Teams
                .OrderBy(t => t.NameOfTeam)
                .ToListAsync();
        }

        public IQueryable<Team> ListTeamsByPlayer()
        {
            return _dbContext.Players
                .Select(p => p.Team)
                .OrderBy(t => t.Id);
        }
    }
}
