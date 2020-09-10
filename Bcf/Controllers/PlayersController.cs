using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bcf.Models;
using Bcf.ViewModels;
using Bcf.Interfaces;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Bcf.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlayersController(IPlayerRepository playerRepository, IWebHostEnvironment webHostEnvironment)
        {
            _playerRepository = playerRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Players
        public async Task<IActionResult> Index(string searchString = "")
        {
            List<Player> players = await _playerRepository.ListAsync(searchString ?? string.Empty);
            List<IndexPlayerViewModel> playersVM = new List<IndexPlayerViewModel>();

            foreach (Player player in players)
            {
                playersVM.Add(new IndexPlayerViewModel()
                {
                    Id = player.Id,
                    FirstName = player.FirstName,
                    LastName = player.LastName,
                    Height = player.Height / 100,
                    Weight = player.Weight,
                    Number = player.Number,
                    Position = player.Position,
                    ProfilePicture = player.ProfilePicture
                }); ;
            }
            return View(playersVM);
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            try
            {
                Player player = await _playerRepository.GetByIdAsync(id.Value);

                if (player == null)
                {
                    return NotFound();
                }

                DetailsPlayerViewModel detailsPlayerVM = new DetailsPlayerViewModel()
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
                return View(detailsPlayerVM);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePlayerViewModel playerVM)
        {
            if (ModelState.IsValid)
            {
                //UploadProfilImage(playerVM);
                Player player = new Player()
                {
                    //Id = playerVM.Id,
                    FirstName = playerVM.FirstName,
                    LastName = playerVM.LastName,
                    Height = playerVM.Height,
                    Weight = playerVM.Weight,
                    Position = playerVM.Position,
                    BirthDate = playerVM.BirthDate,
                    NickName = playerVM.NickName,
                    Number = playerVM.Number,
                    ProfilePicture = UploadOrReplaceProfilImage(playerVM.ProfileImage)
                };
                await _playerRepository.AddAsync(player);
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(playerVM);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            Player player = await _playerRepository.GetByIdAsync(id.Value);

            if (player == null)
            {
                return NotFound();
            }
            EditPlayerViewModel playerViewModel = new EditPlayerViewModel
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
            return View(playerViewModel);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPlayerViewModel playerVM)
        {
            if (id != playerVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Player player = new Player()
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
                        ProfilePicture = UploadOrReplaceProfilImage(playerVM.ProfileImage) ?? playerVM.ProfilePicture
                    };
                    await _playerRepository.UpdateAsync(player);
                    if (playerVM.ProfileImage != null)
                    {
                        DeleteProfilImage(playerVM.ProfilePicture);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_playerRepository.Exist(playerVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(playerVM);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Player player = await _playerRepository.GetByIdAsync(id.Value);

            if (player == null)
            {
                return NotFound();
            }

            DeletePlayerViewModel deletePlayerViewModel = new DeletePlayerViewModel()
            {
                Id = player.Id,
                FullName = player.FullName
            };
            return View(deletePlayerViewModel);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Player player = await _playerRepository.GetByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }
            await _playerRepository.DeleteAsync(player);
            DeleteProfilImage(player.ProfilePicture);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Upload dans le répertoir image une image avec un nom unique
        /// </summary>
        /// <param name="playerVM">Le player model</param>
        private string UploadOrReplaceProfilImage(IFormFile ProfileImage)
        {
            string uniqueFileName = ProfileImage == null ? null : $"{ Guid.NewGuid()}_{ ProfileImage.FileName }";

            if (uniqueFileName != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        /// <summary>
        /// Géère un nom de fichier unique
        /// </summary>
        /// <param name="model">Le modèle contenant l'object FormFile</param>
        /// <returns>Un nom de fichier unique</returns>
        private string GetUniqueFileName(CreatePlayerViewModel model)
        {
            if (model.ProfileImage == null)
            {
                return null;
            }
            return $"{ Guid.NewGuid()}_{ model.ProfileImage.FileName }";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        private void DeleteProfilImage(string profilePicture)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment?.WebRootPath, "images", profilePicture ?? string.Empty);

                // Check if file exists with its full path    
                if (!System.IO.File.Exists(path))
                {
                    return;
                }
                // If file found, delete it    
                System.IO.File.Delete(path);
            }
            catch (IOException ioExp)
            {
                throw ioExp;
            }
        }
    }
}