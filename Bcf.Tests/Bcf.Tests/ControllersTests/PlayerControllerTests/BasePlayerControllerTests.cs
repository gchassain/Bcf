using Bcf.Controllers;
using Bcf.Interfaces;
using Bcf.Models;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.Collections.Generic;

namespace Bcf.Tests.ControllersTests.PlayerControllerTests
{
    public abstract class BasePlayerControllerTests
    {
        protected readonly List<Player> Players;
        protected readonly Mock<IPlayerRepository> MockRepo;
        protected readonly PlayerController PlayerControllerTests;
        private readonly Mock<IWebHostEnvironment> MockWebHostEnvironment;

        protected BasePlayerControllerTests(List<Player> players)
        {
            Players = players;
            MockRepo = new Mock<IPlayerRepository>();
            MockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            MockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("WebRootPath:~/images");
            MockRepo.Setup(repo => repo.ListAsync("")).ReturnsAsync(Players);
            PlayerControllerTests = new PlayerController(MockRepo.Object, MockWebHostEnvironment.Object);
        }
    }
}
