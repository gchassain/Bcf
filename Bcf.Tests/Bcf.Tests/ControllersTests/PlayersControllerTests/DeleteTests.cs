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
    public class DeleteTests : BasePlayersControllerTests
    {
        private static readonly Player PlayerOne = new Player()
        {
            Id = 1
        };
        private static readonly Player PlayerTwo = new Player
        {
            Id = 2
        };

        public DeleteTests() : base(new List<Player>() { PlayerOne, PlayerTwo })
        { }

        [Fact]
        public async Task Delete_Get_WithIdNull_ShouldReturn_NotFound()
        {
            // Act
            IActionResult result = await PlayersControllerTest.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_WithInvalidId_ShouldReturn_NotFound()
        {
            // Act
            IActionResult result = await PlayersControllerTest.Delete(19);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ShouldCall_GetAync_Once()
        {
            // Act
            IActionResult result = await PlayersControllerTest.Delete(1);

            // Assert
            MockRepo.Verify(mock => mock.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Get_ViewModel_ShouldBeOfType_DeletePlayerViewModel()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);

            // Act
            IActionResult result = await PlayersControllerTest.Delete(1);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<DeletePlayerViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Delete_Get_ViewModel_ShouldHave_CorrectProperties()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);

            // Act
            IActionResult result = await PlayersControllerTest.Delete(1);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            DeletePlayerViewModel deletePlayerVM = Assert.IsAssignableFrom<DeletePlayerViewModel>(viewResult.ViewData.Model);
            Assert.Equal(PlayerOne.Id, deletePlayerVM.Id);
            Assert.Equal(PlayerOne.FullName, deletePlayerVM.FullName);
        }

        [Fact]
        public async Task Delete_Post_WithInvalidId_ShouldReturn_NotFound()
        {
            DeletePlayerViewModel deletePlayerVM = new DeletePlayerViewModel();

            // Act
            IActionResult result = await PlayersControllerTest.DeleteConfirmed(deletePlayerVM.Id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Post_ShouldReturn_RedirectToAction_Index()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);
            DeletePlayerViewModel deletePlayerVM = new DeletePlayerViewModel() { Id = PlayerOne.Id };
            // Act
            IActionResult result = await PlayersControllerTest.DeleteConfirmed(deletePlayerVM.Id);

            // Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(PlayersControllerTest.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Delete_Post_ShouldCall_DeleteAsync_Once_IfModelIsValid()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);
            DeletePlayerViewModel deletePlayerVM = new DeletePlayerViewModel() { Id = PlayerOne.Id };

            // Act
            IActionResult result = await PlayersControllerTest.DeleteConfirmed(deletePlayerVM.Id);

            // Assert
            MockRepo.Verify(mock => mock.DeleteAsync(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Post_ShouldCall_DeleteAsync_WithCorrectParameter()
        {
            MockRepo.Setup(repo => repo.GetByIdAsync(PlayerOne.Id)).ReturnsAsync(PlayerOne);
            DeletePlayerViewModel deletePlayerVM = new DeletePlayerViewModel() { Id = PlayerOne.Id, FullName = PlayerOne.FullName };

            // Act
            await PlayersControllerTest.DeleteConfirmed(deletePlayerVM.Id);
            // Assert
            MockRepo.Verify(repo => repo.DeleteAsync(It.Is<Player>(p => p.Id.Equals(PlayerOne.Id))), Times.Once);
        }
    }
}
