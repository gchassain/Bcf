using Bcf.Models;
using Bcf.ViewModels;
using System.Collections.Generic;

namespace Bcf.Services
{
    public interface IPlayerService
    {
        Player Clone(PlayerViewModel model);
        PlayerViewModel CreateViewModel(Player player);
        List<IndexPlayerViewModel> CreateListIndexPlayerViewModel(List<Player> players);
        void UploadProfilImage(PlayerViewModel playerVM);
        void DeleteProfilImage(Player player);
        DeletePlayerViewModel CreateDeletePlayerViewModel(Player player);
        DetailsPlayerViewModel CreateDetailsPlayerViewModel(Player player);
    }
}
