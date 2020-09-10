using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Bcf.Data;
using Bcf.Interfaces;
using System;
using System.Threading.Tasks;
using Bcf.Models;
using System.Linq;
using System.Collections.Generic;

namespace Bcf
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<BcfContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BcfContext")));
            services.AddScoped<IPlayerRepository, PlayerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                IPlayerRepository repository = serviceProvider.GetRequiredService<IPlayerRepository>();

                InitializeDatabaseAsync(repository).Wait();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public async Task InitializeDatabaseAsync(IPlayerRepository repo)
        {
            List<Player> players = await repo.ListAsync("");

            if (!players.Any())
            {
                await repo.SaveChangeAsync(GetPlayersTest());
            }
        }

        public static List<Player> GetPlayersTest()
        {
            List<Player> players = new List<Player>
            {
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
                    ProfilePicture = "michael-jordan.png"
                },
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
                    ProfilePicture = "lebron-james.png"
                }
            };
            return players;
        }
    }
}