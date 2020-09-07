//using Bcf.Controllers;
//using Bcf.Interfaces;
//using Bcf.Models;
//using Bcf.ViewModels;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;

//namespace Bcf.Tests.ControllersTests
//{
//    public class PlayersControllerTests
//    {
//        private readonly Mock<IPlayerRepository> _mockRepo;
//        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
//        public PlayersController _playerController;

//        public PlayersControllerTests()
//        {
//            _mockRepo = new Mock<IPlayerRepository>();
//            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
//            _mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
//        }

//        [Fact]
//        public async Task Index_ListPlayersValid_Return_ViewResult()
//        {
//            _mockRepo.Setup(repo => repo.ListAsync("")).ReturnsAsync(GetTestPlayers());
//            _playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);

//            // Act
//            var result = await _playerController.Index();

//            // Assert
//            Assert.IsAssignableFrom<ViewResult>(result);
//            _mockRepo.Verify(x => x.ListAsync(""), Times.Once);
//        }

//        [Fact]
//        public async Task Index_ListPlayerEmpty_Return_ViewResult()
//        {
//            _mockRepo.Setup(repo => repo.ListAsync("")).ReturnsAsync(new List<Player>());
//            _playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);

//            // Act
//            var result = await _playerController.Index();

//            // Assert
//            Assert.IsAssignableFrom<ViewResult>(result);
//            _mockRepo.Verify(x => x.ListAsync(""), Times.Once);
//        }

//        [Fact]
//        public async Task Detail_PlayerByIdValid_Return_ViewResult()
//        {
//            Player mockPlayer = new Player
//            {
//                Id = 1,
//                FirstName = "LeBron",
//                LastName = "James",
//                NickName = "The king",
//                Height = 206,
//                Weight = 113,
//                BirthDate = new DateTime(1984, 12, 30),
//                Number = 23,
//                Position = Enums.PlayerPositionsEnum.POWER_FORWARD,
//                ProfilePicture = "lebron-james.png"
//            };
//            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockPlayer);
//            _playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);

//            // Act
//            var result = await _playerController.Details(1);

//            // Assert
//            Assert.IsAssignableFrom<ViewResult>(result);
//            _mockRepo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);

//        }

//        [Fact]
//        public async Task Detail_PlayerByIdInvalid_Return_NotFound()
//        {
//           // _mockRepo.Setup(repo => repo.ListAsync("")).ReturnsAsync(GetTestPlayers());
//           // _playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);

//            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("Player with that ID was not found!"));
//            _playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);


//            var list = await _playerController.Index();
//            // Act
//            var result = await _playerController.Details(10);

//            // Assert
//            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
//            _mockRepo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
//        }

//        [Fact]
//        public async Task Detail_PlayerByIdNull_Return_BadRequest()
//        {
//            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("Player with that ID was not found!"));
//            _playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);

//            // Act
//            var result = await _playerController.Details(null);

//            // Assert
//            Assert.IsAssignableFrom<BadRequestResult>(result);
//            _mockRepo.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
//        }

//        [Fact]
//        public async Task Create_PlayerValid_RedirectAction_Index()
//        {
//            PlayerViewModel mockPlayer = new PlayerViewModel
//            {
//                FirstName = "LeBron",
//                LastName = "James",
//                NickName = "The king",
//                Height = 206,
//                Weight = 113,
//                BirthDate = new DateTime(1984, 12, 30),
//                Number = 23,
//                Position = Enums.PlayerPositionsEnum.POWER_FORWARD,
//                ProfilePicture = "lebron-james.png"
//            };
//            _mockRepo.Setup(repo => repo.ListAsync("")).ReturnsAsync(GetTestPlayers());
//            _playerController = new PlayersController(_mockRepo.Object, _mockWebHostEnvironment.Object);

//            // Act
//            var result = await _playerController.Create(mockPlayer);

//            // Assert
//            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

//            Assert.Null(redirectToActionResult.ControllerName);
//            Assert.Equal("Index", redirectToActionResult.ActionName);
//            _mockRepo.Verify();
//        }

//        private List<Player> GetTestPlayers()
//        {
//            List<Player> players = new List<Player>
//            {
//                new Player
//                {
//                    Id = 1,
//                    FirstName = "LeBron",
//                    LastName = "James",
//                    NickName = "The king",
//                    Height = 206,
//                    Weight = 113,
//                    BirthDate = new DateTime(1984, 12, 30),
//                    Number = 23,
//                    Position = Enums.PlayerPositionsEnum.POWER_FORWARD,
//                    ProfilePicture = "lebron-james.png"
//                },
//                new Player
//                {
//                    Id = 2,
//                    FirstName = "Michael",
//                    LastName = "Jordan",
//                    NickName = "His Airness",
//                    Height = 198,
//                    Weight = 98,
//                    BirthDate = new DateTime(1963, 02, 17),
//                    Number = 23,
//                    Position = Enums.PlayerPositionsEnum.SMALL_FORWARD,
//                    ProfilePicture = "michael-jordan.png"
//                }
//            };
//            return players;
//        }
//    }
//}
