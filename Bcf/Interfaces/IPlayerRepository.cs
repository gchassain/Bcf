using Bcf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bcf.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> GetByIdAsync(int id);
        Task<List<Player>> ListAsync(string searchString);
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
        bool Exist(int id);
        Task SaveChangeAsync(List<Player> players);
    }
}
