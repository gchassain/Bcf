using Bcf.Controllers;
using Bcf.Models;
using Bcf.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bcf.Tests.ControllersTests.PlayersControllerTests
{
    public class CreateTests : BasePlayersControllerTests
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

        public CreateTests() : base (new List<Player>() { PlayerOne, PlayerTwo })
        { }

        [Fact]
        public void Create_Get_ShouldHaveNo_ViewModel()
        {
            // Act
            var result = PlayersControllerTest.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Create_Post_ShouldReturn_PlayerViewModel_IfModelIsInvalid()
        {
            PlayerViewModel playerVM = new PlayerViewModel();
            PlayersControllerTest.ModelState.AddModelError("error", "testerror");

            // Act
            var result = await PlayersControllerTest.Create(playerVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<PlayerViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Create_Post_ShouldReturn_RedirectToActionIndex_IfModelIsValid()
        {
            var model = new PlayerViewModel();

            // Act
            var result = await PlayersControllerTest.Create(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(PlayersController.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Create_Post_ShouldCall_AddItemAsyncOnce_IfModelIsValid()
        {
            var model = new PlayerViewModel() { FirstName = nameof(Create_Post_ShouldCall_AddItemAsyncOnce_IfModelIsValid) };

            // Act
            await PlayersControllerTest.Create(model);

            // Verify
            MockRepo.Verify(mock => mock.AddAsync(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public async Task Create_Post_ShouldCall_AddItemAsyncWithCorrectParameter_IfModelIsValid()
        {
            Player player = new Player() { FirstName = nameof(Create_Post_ShouldCall_AddItemAsyncWithCorrectParameter_IfModelIsValid) };
            PlayerViewModel model = new PlayerViewModel() { FirstName = player.FirstName };

            // Act
            await PlayersControllerTest.Create(model);

            // Assert
            MockRepo.Verify(mock => mock.AddAsync(It.Is<Player>(i => i.FirstName.Equals(player.FirstName))), Times.Once);
        }

    }
}
