namespace PLF_Football.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.ApplicationUsers;
    using PLF_Football.Web.ViewModels.Fixtures;
    using PLF_Football.Web.ViewModels.UserGame;

    public class UsersController : BaseController
    {
        private readonly IUserGamesService userGamesService;
        private readonly IUsersService usersService;
        private readonly IFixtureService fixtureService;
        private readonly IFixtureScraperService fixtureScraperService;

        public UsersController(
            IUserGamesService userGamesService,
            IUsersService usersService,
            IFixtureService fixtureService,
            IFixtureScraperService fixtureScraperService)
        {
            this.userGamesService = userGamesService;
            this.usersService = usersService;
            this.fixtureService = fixtureService;
            this.fixtureScraperService = fixtureScraperService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();

            var lastOfOldMatchdays = nextMatchday - 3;
            var viewModel = this.usersService.GetUserById<UserIndexPageViewModel>(userId);

            viewModel.Id = userId;
            viewModel.Teams = this.userGamesService
                        .GetUserGamesByUserIdAfterSpecificMatchday<UserGameTeamViewModel>(userId, lastOfOldMatchdays);
            viewModel.LastStartedMatchday = nextMatchday - 1;

            foreach (var matchday in viewModel.Teams)
            {
                if (matchday.Matchday < nextMatchday)
                {
                    matchday.IsMatchdayStarted = true;
                }
            }

            var fixtures = this.fixtureService.GetFixturesAfterSpecificAndBeforeOrOnNextMatchday<FixtureBasicViewModel>(lastOfOldMatchdays, nextMatchday);

            viewModel.Fixtures = fixtures;

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllTeams(string userId = null)
        {
            if (userId == null)
            {
                userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var viewModel = this.usersService.GetUserById<UserAllTeamsViewModel>(userId);
            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();
            foreach (var matchday in viewModel.Teams)
            {
                if (matchday.Matchday < nextMatchday)
                {
                    matchday.IsMatchdayStarted = true;
                }
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Team(string userId, int matchday)
        {
            var viewModel = this.userGamesService.GetUserGame<UserGameTeamViewModel>(userId, matchday);
            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();
            if (viewModel.Matchday < nextMatchday)
            {
                viewModel.IsMatchdayStarted = true;
            }

            return this.View(viewModel);
        }
    }
}
