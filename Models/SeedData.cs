using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bcf.Data;
using System;
using System.Linq;

namespace Bcf.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BcfContext(serviceProvider.GetRequiredService<DbContextOptions<BcfContext>>()))
            {
                // Look for any movies.
                if (context.Players.Any())
                {
                    return;   // DB has been seeded
                }

                context.Players.AddRange(
                    new Player
                    {
                        FirstName = "LeBron",
                        LastName = "James",
                        NickName = "The king",
                        Height = 206,
                        Weight = 113,
                        BirthDate = new DateTime(1984,12,30),
                        Number = 23,
                        Position = Enums.PlayerPositionsEnum.POWER_FORWARD,
                        ProfilePicture = "lebron-james.png"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}