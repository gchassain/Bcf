using Bcf.Models;
using Bcf.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bcf.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlayerService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Player Clone(PlayerViewModel playerVM)
        {
            return new Player()
            {
                Id = playerVM.Id,
                FirstName = playerVM.FirstName,
                LastName = playerVM.LastName,
                Height = playerVM.Height,
                Weight = playerVM.Weight,
                Position = playerVM.Position,
                BirthDate = playerVM.BirthDate,
                NickName = playerVM.NickName,
                Number = playerVM.Number,
                ProfilePicture = playerVM.ProfilePicture
            };
        }

        public DeletePlayerViewModel CreateDeletePlayerViewModel(Player player)
        {
            return new DeletePlayerViewModel()
            {
                Id = player.Id,
                FullName = player.FullName
            };
        }

        public DetailsPlayerViewModel CreateDetailsPlayerViewModel(Player player)
        {
            return new DetailsPlayerViewModel()
            {
                Id = player.Id,
                FullName = player.FullName,
                Height = player.Height / 100,
                Weight = player.Weight,
                Number = player.Number,
                Position = player.Position,
                ProfilePicture = player.ProfilePicture,
                BirthDate = player.BirthDate
            };
        }

        public List<IndexPlayerViewModel> CreateListIndexPlayerViewModel(List<Player> players)
        {
            List<IndexPlayerViewModel> playersVM = new List<IndexPlayerViewModel>();

            foreach(Player player in players)
            {
                playersVM.Add(new IndexPlayerViewModel()
                {
                    Id = player.Id,
                    FullName = player.FullName,
                    Height = player.Height / 100,
                    Weight = player.Weight,
                    Number = player.Number,
                    Position = player.Position,
                    ProfilePicture = player.ProfilePicture
                });
            }
            return playersVM;
        }

        public PlayerViewModel CreateViewModel(Player player)
        {
            return new PlayerViewModel
            {
                Id = player.Id,
                BirthDate = player.BirthDate,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Height = player.Height,
                Weight = player.Weight,
                NickName = player.NickName,
                Number = player.Number,
                Position = player.Position,
                ProfilePicture = player.ProfilePicture
            };
        }

        public void DeleteProfilImage(Player player)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "images",  player.ProfilePicture ?? string.Empty);

                // Check if file exists with its full path    
                if (File.Exists(path))
                {
                    // If file found, delete it    
                    File.Delete(path);
                    Console.WriteLine("File deleted.");
                }
               /* else
                {
                    Console.WriteLine("File not found");
                }*/
            }
            catch (IOException ioExp)
            {
                throw ioExp;
            }
        }

        /// <summary>
        /// Upload dans le répertoir image une image avec un nom unique
        /// </summary>
        /// <param name="playerVM">Le player model</param>
        public void UploadProfilImage(PlayerViewModel playerVM)
        {
            string uniqueFileName = GetUniqueFileName(playerVM);

            if (uniqueFileName != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    playerVM.ProfileImage.CopyTo(fileStream);
                }
                playerVM.ProfilePicture = uniqueFileName;
            }
        }

        /// <summary>
        /// Géère un nom de fichier unique
        /// </summary>
        /// <param name="model">Le modèle contenant l'object FormFile</param>
        /// <returns>Un nom de fichier unique</returns>
        private string GetUniqueFileName(PlayerViewModel model)
        {
            if (model.ProfileImage == null)
            {
                return null;
            }
            return $"{ Guid.NewGuid()}_{ model.ProfileImage.FileName }";
        }
    }
}
