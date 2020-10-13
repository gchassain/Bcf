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
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Bcf.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IBcfRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlayerController(IBcfRepository playerRepository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = playerRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Player
        public async Task<IActionResult> Index(string playerTeam, string searchString)
        {
            IQueryable<string> teamsQuery = _repository.ListTeamsByPlayer().Select(t => t.NameOfTeam);
            List<Player> players = _repository.ListPlayers().ToList();            

            if (!string.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.LastName.Contains(searchString) ||
                                             p.FirstName.Contains(searchString))
                    .ToList();
            }
            if (!string.IsNullOrEmpty(playerTeam))
            {
                players = players.Where(p => p.Team.NameOfTeam == playerTeam)
                    .ToList();
            }

            List<PlayerViewModel> playersVM = players.Select(player => new PlayerViewModel()
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Height = player.Height / 100,
                Weight = player.Weight,
                Number = player.Number,
                Position = player.Position,
                ProfilePicture = player.ProfilePicture,
                NameOfTeam = player?.Team?.NameOfTeam
            }).ToList();

            IndexViewModel indexVM = new IndexViewModel()
            {
                PlayersVM = playersVM,
                Teams = new SelectList(await teamsQuery.Distinct().ToListAsync())
            };
            return View(indexVM);
        }

        // GET: Player/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            try
            {
                Player player = await _repository.GetByIdAsync(id.Value);

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
                    BirthDate = player.BirthDate,
                    NameOfTeam = player?.Team?.NameOfTeam
                };
                return View(detailsPlayerVM);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

         // GET: Player/Create
        public async Task<IActionResult> Create()
        {
            List<Team> teams = await _repository.ListTeamsAsync();
            CreatePlayerViewModel viewModel = new CreatePlayerViewModel
            {
                Teams = teams.Select(t =>
                                        new SelectListItem()
                                        {
                                            Value = t.Id.ToString(),
                                            Text = t.NameOfTeam
                                        }).ToList()
            };
            return View(viewModel);
        }

        // POST: Player/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName, LastName, Position, Number, Height, Weight, NickName, BirthDate, ProfilPicture, ProfileImage, TeamId")]CreatePlayerViewModel playerVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Player player = new Player()
                    {
                        FirstName = playerVM.FirstName,
                        LastName = playerVM.LastName,
                        Height = playerVM.Height,
                        Weight = playerVM.Weight,
                        Position = playerVM.Position,
                        BirthDate = playerVM.BirthDate,
                        NickName = playerVM.NickName,
                        Number = playerVM.Number,
                        ProfilePicture = UploadOrReplaceProfilImage(playerVM.ProfileImage),
                        TeamId = playerVM.TeamId
                    };
                    await _repository.AddAsync(player);
                    return RedirectToAction(actionName: nameof(Index));
                }
            }
            catch (DataException /*dEx*/)
            {
                //Log the error (uncomment dEx variable name and add a line here to write a log.
                ModelState.AddModelError("", "Impossible d'enregistrer les modifications. Réessayez, et si le problème persiste, consultez votre administrateur système.");
            }
            return View(playerVM);
        }

        // GET: Player/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            Player player = await _repository.GetByIdAsync(id.Value);
            List<Team> teams = await _repository.ListTeamsAsync();

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
                ProfilePicture = player.ProfilePicture,
                TeamId = player.TeamId,
                Teams = teams.Select(t =>
                        new SelectListItem()
                        {
                            Value = t.Id.ToString(),
                            Text = t.NameOfTeam
                        }).ToList()
            };
            return View(playerViewModel);
        }

        // POST: Player/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [FromForm] EditPlayerViewModel playerVM)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
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
                        TeamId = playerVM.TeamId,
                        Height = playerVM.Height,
                        Weight = playerVM.Weight,
                        Position = playerVM.Position,
                        BirthDate = playerVM.BirthDate,
                        NickName = playerVM.NickName,
                        Number = playerVM.Number,
                        ProfilePicture = UploadOrReplaceProfilImage(playerVM.ProfileImage) ?? playerVM.ProfilePicture
                    };
                    await _repository.UpdateAsync(player);
                    if (playerVM.ProfileImage != null)
                    {
                        DeleteProfilImage(playerVM.ProfilePicture);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.Exist(playerVM.Id))
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

        // GET: Player/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Player player = await _repository.GetByIdAsync(id.Value);

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

        // POST: Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Player player = await _repository.GetByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(player);
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