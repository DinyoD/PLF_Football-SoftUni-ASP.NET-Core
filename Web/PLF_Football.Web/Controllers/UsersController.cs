namespace PLF_Football.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.ApplicationUsers;
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
            var viewModel = this.usersService.GetUserById<UserProfileViewModel>(userId);
            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();
            foreach (var matchday in viewModel.Teams)
            {
                if (matchday.Matchday >= nextMatchday)
                {
                    matchday.IsMatchdayStarted = false;
                }
                else
                {
                    matchday.IsMatchdayStarted = true;
                }
            }

            foreach (var player in viewModel.Teams.Select(x => x.Team))
            {

            }

            return this.View(viewModel);
        }

        public IActionResult Team(string userId, int matchday)
        {
            var viewModel = this.userGamesService.GetUserGame<UserGameTeamViewModel>(userId, matchday);
            return this.View(viewModel);
        }
    }
}
