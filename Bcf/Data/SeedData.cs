using Bcf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Bcf.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BcfContext(serviceProvider.GetRequiredService<DbContextOptions<BcfContext>>()))
            {
                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                        new Team () { NameOfTeam = "Equipe 1" },
                        new Team () { NameOfTeam = "Equipe 2" });
                    context.SaveChanges();
                }
                if (!context.Players.Any())
                {
                    context.Players.AddRange(
                    #region MichaelJordan
                        new Player()
                        {
                            FirstName = "Michael",
                            LastName = "Jordan",
                            NickName = "His Airness",
                            Height = 198,
                            Weight = 98,
                            BirthDate = new DateTime(1963, 02, 17),
                            Number = 23,
                            Position = Enums.PlayerPositionsEnum.SMALL_FORWARD,
                            ProfilePicture = "michael-jordan.png",
                            TeamId = 1
                        },
                    #endregion
                    #region LebronJames
                        new Player()
                        {
                            FirstName = "LeBron",
                            LastName = "James",
                            NickName = "The king",
                            Height = 206,
                            Weight = 113,
                            BirthDate = new DateTime(1984, 12, 30),
                            Number = 23,
                            Position = Enums.PlayerPositionsEnum.POWER_FORWARD,
                            ProfilePicture = "lebron-james.png",
                            TeamId = 2
                        });
                    #endregion
                    context.SaveChanges();
                }
            }
        }
    }
}
