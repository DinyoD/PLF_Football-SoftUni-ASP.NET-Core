namespace PLF_Football.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Data.Models;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;

    [Authorize]
    public class UserGamesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserGamesService userGamesService;
        private readonly IFixtureService fixtureService;
        private readonly IPlayersService playersService;
        private readonly IFixtureScraperService fixtureScraperService;

        public UserGamesController(
            UserManager<ApplicationUser> userManager,
            IUserGamesService userGamesService,
            IFixtureService fixtureService,
            IPlayersService playersService,
            IFixtureScraperService fixtureScraperService)
        {
            this.userManager = userManager;
            this.userGamesService = userGamesService;
            this.fixtureService = fixtureService;
            this.playersService = playersService;
            this.fixtureScraperService = fixtureScraperService;
        }

        public async Task<IActionResult> AddPlayer(int id)
        {
            var currUser = await this.userManager.GetUserAsync(this.User);
            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();

            var userGameId = this.userGamesService.GetUserGameIdByUserAndMatchday(currUser.Id, nextMatchday);
            var userGameUserId = this.userGamesService.GetUserIdByUserGameId(userGameId);
            if (currUser.Id != userGameUserId)
            {
                return this.Unauthorized();
            }

            if (userGameId == 0 && nextMatchday < 39)
            {
                userGameId = await this.userGamesService.CreateUserGameAsync(currUser.Id, nextMatchday);
            }

            var player = this.playersService.GetPlayerById(id);
            var playersInTeam = this.userGamesService.GetUserGameMatchTeamPlayers(userGameId);
            var teamPrice = playersInTeam.Sum(x => x.Price);

            if (playersInTeam.Count >= 11)
            {
                return this.RedirectToPage("Error - team is full");
            }
            else if (playersInTeam.Contains(player))
            {
                return this.RedirectToAction("User", "Error");
            }
            else if (currUser.ClubId == player.ClubId
                && playersInTeam.Where(x => x.ClubId == currUser.ClubId).Count() >= 5)
            {
                return this.RedirectToPage("Error - already five local");
            }
            else if (currUser.ClubId != player.ClubId
                && playersInTeam.Where(x => x.ClubId != currUser.ClubId).Count() >= 6)
            {
                return this.RedirectToPage("Error - already six foreign");
            }

            // Gk
            else if (player.PositionId == 4
                && playersInTeam.Where(x => x.PositionId == 4).Count() >= 1)
            {
                return this.RedirectToPage("Error - already one GK");
            }

            // Def
            else if (player.PositionId == 3
                && playersInTeam.Where(x => x.PositionId == 3).Count() >= 4)
            {
                return this.RedirectToPage("Error - already 4 Defs");
            }

            // Midf
            else if (player.PositionId == 2
                && playersInTeam.Where(x => x.PositionId == 2).Count() >= 4)
            {
                return this.RedirectToPage("Error - already 4 Midf");
            }

            // Forw
            else if (player.PositionId == 1
                && playersInTeam.Where(x => x.PositionId == 1).Count() >= 2)
            {
                return this.RedirectToPage("Error - already 2 Forw");
            }
            else if (GlobalConstants.UserBudget - teamPrice < player.Price)
            {
                return this.RedirectToPage("Error - price too hight");
            }
            else
            {
                await this.userGamesService.AddPlayerToUserGameAsync(id, userGameId);
                return this.Redirect($"/Users/Team/?matchday={nextMatchday}");
            }
        }

        public async Task<IActionResult> Remove(int playerId, int userGameId)
        {
            var currUser = await this.userManager.GetUserAsync(this.User);
            var userGameUserId = this.userGamesService.GetUserIdByUserGameId(userGameId);
            if (currUser.Id != userGameUserId)
            {
                return this.Unauthorized();
            }

            await this.userGamesService.RemovePlayerFromUserGameAsync(playerId, userGameId);
            var matchday = this.userGamesService.GetMatchdayByuserGameId(userGameId);
            return this.Redirect($"/Users/Team/?matchday={matchday}");
        }
    }
}
