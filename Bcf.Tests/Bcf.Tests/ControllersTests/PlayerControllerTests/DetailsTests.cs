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
    public class DetailsTests : BasePlayerControllerTests
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
            ProfilePicture = "lebron-james.png",
            Team = new Team() { NameOfTeam = "Equipe 1" }
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
            ProfilePicture = "michael-jordan.png",
            Team = new Team() { NameOfTeam = "Equipe 2" }
        };

        public DetailsTests() : base(new List<Player>() { PlayerOne, PlayerTwo })
        { }

        [Fact]
        public async Task Details_Get_WithIdNull_ShouldReturn_NotFound()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_Get_WithInvalidId_ShouldReturn_NotFound()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Details(19);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_Get_ShouldCall_GetAsync_Once()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Details(1);

            MockRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Details_Get_ViewModel_ShouldBeOfType_DetailsPlayerViewModel()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);

            // Act
            IActionResult result = await PlayerControllerTests.Details(PlayerOne.Id);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<DetailsPlayerViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Details_Get_ViewModel_ShouldHaveCorrectProperties()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);

            // Act
            IActionResult result = await PlayerControllerTests.Details(PlayerOne.Id);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            DetailsPlayerViewModel viewModel = Assert.IsAssignableFrom<DetailsPlayerViewModel>(viewResult.ViewData.Model);
            Assert.Equal(PlayerOne.Id, viewModel.Id);
            Assert.Equal(PlayerOne.FullName, viewModel.FullName);
            Assert.Equal(PlayerOne.Number, viewModel.Number);
            Assert.Equal(PlayerOne.Height/100, viewModel.Height);
            Assert.Equal(PlayerOne.Weight, viewModel.Weight);
            Assert.Equal(PlayerOne.BirthDate, viewModel.BirthDate);
            Assert.Equal(PlayerOne.Position, viewModel.Position);
            Assert.Equal(PlayerOne.ProfilePicture, viewModel.ProfilePicture);
        }      
    }
}
