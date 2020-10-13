﻿using Bcf.Models;
using Bcf.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bcf.Tests.ControllersTests.PlayerControllerTests
{
    public class IndexTests : BasePlayerControllerTests
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

        public IndexTests() : base(new List<Player>() {  PlayerOne, PlayerTwo })
        { }

        [Fact]
        public async Task Index_GetViewModel_ShouldBeOfType_IEnumerablePlayer()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<PlayerViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Index_Get_ShouldReturn_ListOfPlayers()
        {
            // Act
            IActionResult result = await PlayerControllerTests.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PlayerViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }
    }
}