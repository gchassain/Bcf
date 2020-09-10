using Bcf.Controllers;
using Bcf.Models;
using Bcf.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bcf.Tests.ControllersTests.PlayerControllerTests
{
    public class EditTests : BasePlayerControllerTests
    {
        private static readonly Player PlayerOne = new Player()
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            NickName = "The king",
            Height = 206,
            Weight = 113,
            BirthDate = new DateTime(1984, 12, 30),
            Number = 23,
            Position = Enums.PlayerPositionsEnum.POWER_FORWARD,
            ProfilePicture = "lebron-james.png"
        };
        private static readonly Player PlayerTwo = new Player
        {
            Id = 2,
            FirstName = "Michael",
            LastName = "Jordan",
            NickName = "His Airness",
            Height = 198,
            Weight = 98,
            BirthDate = new DateTime(1963, 02, 17),
            Number = 23,
            Position = Enums.PlayerPositionsEnum.SMALL_FORWARD,
            ProfilePicture = "michael-jordan.png"
        };

        public EditTests() : base (new List<Player>() { PlayerOne, PlayerTwo })
        { }

        [Fact]
        public async Task Edit_Get_WithIdNull_ShouldReturn_NotFound()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_WithInvalidId_ShouldReturn_NotFound()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Edit(19);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ShouldCall_GetAsync_Once()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Edit(1);

            MockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Edit_Get_ViewModel_ShouldBeOfType_PlayerViewModel()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);

            // Act
            IActionResult result = await PlayerControllerTests.Edit(PlayerOne.Id);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditPlayerViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_Get_ViewModel_ShouldHave_CorrectProperties()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);

            // Act
            IActionResult result = await PlayerControllerTests.Edit(PlayerOne.Id);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            EditPlayerViewModel viewModel = Assert.IsAssignableFrom<EditPlayerViewModel>(viewResult.ViewData.Model);
            Assert.Equal(PlayerOne.FirstName, viewModel.FirstName);
            Assert.Equal(PlayerOne.LastName, viewModel.LastName);
            Assert.Equal(PlayerOne.Number, viewModel.Number);
            Assert.Equal(PlayerOne.Height, viewModel.Height);
            Assert.Equal(PlayerOne.Weight, viewModel.Weight);
            Assert.Equal(PlayerOne.BirthDate, viewModel.BirthDate);
            Assert.Equal(PlayerOne.NickName, viewModel.NickName);
            Assert.Equal(PlayerOne.Position, viewModel.Position);
            Assert.Equal(PlayerOne.ProfilePicture, viewModel.ProfilePicture);
        }

        [Fact]
        public async Task Edit_Post_ShouldReturn_PlayerViewModel_IfModelIsInvalid()
        {
            EditPlayerViewModel model = new EditPlayerViewModel() { Id = PlayerOne.Id };

            PlayerControllerTests.ModelState.AddModelError("error", "testerror");

            // Act
            IActionResult result = await PlayerControllerTests.Edit(PlayerOne.Id, model);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EditPlayerViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_Post_ShouldReturn_RedirectToActionIndex_IfModelIsValid()
        {
            EditPlayerViewModel model = new EditPlayerViewModel() { Id = PlayerOne.Id };

            // Act
            IActionResult result = await PlayerControllerTests.Edit(PlayerOne.Id, model);

            // Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(PlayerController.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ShouldCall_UpdateAsync_Once_IfModelIsValid()
        {
            EditPlayerViewModel model = new EditPlayerViewModel() { Id = PlayerOne.Id };

            // Act
            IActionResult result = await PlayerControllerTests.Edit(PlayerOne.Id, model);

            MockRepo.Verify(mock => mock.UpdateAsync(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public async Task Edit_Post_ShouldCall_UpdateAsync_WithCorrectParameter_IfModelIsValid()
        {
            Player player = new Player() { Id = PlayerOne.Id, FirstName = nameof(Edit_Post_ShouldCall_UpdateAsync_WithCorrectParameter_IfModelIsValid) };
            EditPlayerViewModel model = new EditPlayerViewModel() { Id = player.Id, FirstName = player.FirstName };

            await PlayerControllerTests.Edit(player.Id, model);

            MockRepo.Verify(mock => mock.UpdateAsync(It.Is<Player>(p => p.FirstName.Equals(player.FirstName) && p.Id.Equals(player.Id))), Times.Once);
        }
    }
}
