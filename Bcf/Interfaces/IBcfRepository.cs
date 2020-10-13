using Bcf.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcf.Interfaces
{
    public interface IBcfRepository
    {
        // PLAYERS
        Task<Player> GetByIdAsync(int id);
        Task<List<Player>> ListPlayersAsync();
        IQueryable<Player> ListPlayers();
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
        bool Exist(int id);
        //Task SaveChangeAsync(Player player);

        // TEAMS
        Task<List<Team>> ListTeamsAsync();
        IQueryable<Team> ListTeamsByPlayer();
    }
}
