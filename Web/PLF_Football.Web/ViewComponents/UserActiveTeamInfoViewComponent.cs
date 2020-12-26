namespace PLF_Football.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Data.Models;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Players;
    using PLF_Football.Web.ViewModels.ViewComponents;

    public class UserActiveTeamInfoViewComponent : ViewComponent
    {
        private readonly IUserGamesService userGamesService;
        private readonly IFixtureScraperService fixtureScraperService;
        private readonly IPlayersService playersService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserActiveTeamInfoViewComponent(
            IUserGamesService userGamesService,
            IFixtureScraperService fixtureScraperService,
            IPlayersService playersService,
            UserManager<ApplicationUser> userManager)
        {
            this.userGamesService = userGamesService;
            this.fixtureScraperService = fixtureScraperService;
            this.playersService = playersService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.userManager.GetUserId(this.UserClaimsPrincipal);
            var firstNotStartedmatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();
            var userGameId = this.userGamesService.GetUserGameIdByUserAndMatchday(userId, firstNotStartedmatchday);
            var playersId = this.userGamesService.GetPlayersIdsByUserGameId(userGameId);
            var team = new List<PlayerForUserActiveTeamViewModel>();
            foreach (var id in playersId)
            {
                var player = this.playersService.GetPlayerById<PlayerForUserActiveTeamViewModel>(id);
                team.Add(player);
            }

            var viewModel = new UserActiveTeamViewModel
            {
                UserId = userId,
                Matchday = firstNotStartedmatchday,
                MatchdayTeam = team,
                Budget = GlobalConstants.UserBudget - team.Sum(x => x.Price),
                PlayersInTeam = team.Count,
            };

            return this.View(viewModel);
        }
    }
}
