using Bcf.Controllers;
using Bcf.Interfaces;
using Bcf.Models;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.Collections.Generic;

namespace Bcf.Tests.ControllersTests.PlayersControllerTests
{
    public abstract class BasePlayersControllerTests
    {
        protected readonly List<Player> Players;
        protected readonly Mock<IPlayerRepository> MockRepo;
        protected readonly PlayersController PlayersControllerTest;
        private readonly Mock<IWebHostEnvironment> MockWebHostEnvironment;

        protected BasePlayersControllerTests(List<Player> players)
        {
            Players = players;
            MockRepo = new Mock<IPlayerRepository>();
            MockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            MockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("WebRootPath:~/images");
            MockRepo.Setup(repo => repo.ListAsync("")).ReturnsAsync(Players);
            PlayersControllerTest = new PlayersController(MockRepo.Object, MockWebHostEnvironment.Object);
        }
    }
}
