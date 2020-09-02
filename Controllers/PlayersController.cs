using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bcf.Models;
using Bcf.ViewModels;
using Bcf.Interfaces;
using Bcf.Services;
using System.Collections.Generic;

namespace Bcf.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerRepository playerRepository, IPlayerService playerService)
        {
            _playerRepository = playerRepository;
            _playerService = playerService;
        }

        // GET: Players
        public async Task<IActionResult> Index(string searchString = "")
        {
            List<Player> players = await _playerRepository.ListAsync(searchString);

            return View(_playerService.CreateListIndexPlayerViewModel(players));
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
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
            return View(_playerService.CreateDetailsPlayerViewModel(player));
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
        public async Task<IActionResult> Create(PlayerViewModel playerVM)
        {
            if (ModelState.IsValid)
            {                
                _playerService.UploadProfilImage(playerVM);
                await _playerRepository.AddAsync(_playerService.Clone(playerVM));
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
            return View(_playerService.CreateViewModel(player));
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlayerViewModel playerVM)
        {
            if (id != playerVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Player player = await _playerRepository.GetByIdAsync(playerVM.Id);

                    _playerService.UploadProfilImage(playerVM);

                    Player player = _playerService.Clone(playerVM);

                    await _playerRepository.UpdateAsync(player);
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
            return View(_playerService.CreateDeletePlayerViewModel(player));
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
            _playerService.DeleteProfilImage(player);
            return RedirectToAction(nameof(Index));
        }
    }
}
