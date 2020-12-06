namespace PLF_Football.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Data;

    public class UserGamesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserGamesService userGamesService;
        private readonly IFixtureService fixtureService;
        private readonly IPlayersService playersService;

        public UserGamesController(
            UserManager<ApplicationUser> userManager,
            IUserGamesService userGamesService,
            IFixtureService fixtureService,
            IPlayersService playersService)
        {
            this.userManager = userManager;
            this.userGamesService = userGamesService;
            this.fixtureService = fixtureService;
            this.playersService = playersService;
        }

        public async Task<IActionResult> AddPlayer(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var nextMatchday = this.fixtureService.GetNextMatchday();
            var userLastMatchday = 0;

            if (user.Games.Count > 0)
            {
                userLastMatchday = this.userGamesService.GetUserLastMatchdayInUserGames(user.Id);
            }

            if (nextMatchday != userLastMatchday)
            {
                await this.userGamesService.CreateUserGameAsync(user.Id, nextMatchday);
            }

            var playersInTeam = this.userGamesService.GetPlayersInLastTeam(user.Id);

            if (playersInTeam.Count >= 11)
            {
                return this.RedirectToPage("Error - team is full");
            }

            var player = this.playersService.GetPlayerById(id);

            if (user.ClubId == player.ClubId
                && playersInTeam.Where(x => x.ClubId == user.FavoriteTeam.Id).Count() >= 5)
            {
                return this.RedirectToPage("Error - already five local");
            }

            if (user.ClubId != player.ClubId
                && playersInTeam.Where(x => x.ClubId != user.FavoriteTeam.Id).Count() >= 6)
            {
                return this.RedirectToPage("Error - already six foreign");
            }

            // Gk
            if (player.PositionId == 1
                && playersInTeam.Where(x => x.PositionId == 1).Count() >= 1)
            {
                return this.RedirectToPage("Error - already one GK");
            }

            // Def
            if (player.PositionId == 2
                && playersInTeam.Where(x => x.PositionId == 2).Count() >= 4)
            {
                return this.RedirectToPage("Error - already 4 Defs");
            }

            // Midf
            if (player.PositionId == 3
                && playersInTeam.Where(x => x.PositionId == 3).Count() >= 4)
            {
                return this.RedirectToPage("Error - already 4 Midf");
            }

            // Forw
            if (player.PositionId == 4
                && playersInTeam.Where(x => x.PositionId == 4).Count() >= 2)
            {
                return this.RedirectToPage("Error - already 2 Forw");
            }

            await this.userGamesService.AddPlayerToUserGameAsync(id);

            return this.Redirect($"Users/Team/{nextMatchday}");
        }
    }
}
