using Bcf.Controllers;
using Bcf.Interfaces;
using Bcf.Models;
using Bcf.Services;
using Bcf.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bcf.Tests.ControllersTests
{
    public class PlayersControllerTests
    {
        private readonly Mock<IPlayerRepository> _mockRepo;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;

        public PlayersControllerTests()
        {
            _mockRepo = new Mock<IPlayerRepository>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfPlayers()
        {
            _mockRepo.Setup(repo => repo.ListAsync("")).ReturnsAsync(GetTestPlayers());
            _mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            PlayersController playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);
            IActionResult result = await playerController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<IndexPlayerViewModel>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        private List<Player> GetTestPlayers()
        {
            List<Player> players = new List<Player>
            {
                new Player
                {
                    FirstName = "LeBron",
                    LastName = "James",
                    NickName = "The king",
                    Height = 206,
                    Weight = 113,
                    BirthDate = new DateTime(1984, 12, 30),
                    Number = 23,
                    Position = Enums.PlayerPositionsEnum.POWER_FORWARD,
                    ProfilePicture = "lebron-james.png"
                }
            };
            return players;
        }
    }
}
