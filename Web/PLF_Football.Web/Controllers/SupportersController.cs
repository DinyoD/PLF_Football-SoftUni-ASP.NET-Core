namespace PLF_Football.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Supporters;

    public class SupportersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IUserGamesService userGamesService;
        private readonly IPlayersPointsService playersPointsService;
        private readonly IFixtureScraperService fixtureScraperService;

        public SupportersController(
            IUsersService usersService,
            IUserGamesService userGamesService,
            IPlayersPointsService playersPointsService,
            IFixtureScraperService fixtureScraperService)
        {
            this.usersService = usersService;
            this.userGamesService = userGamesService;
            this.playersPointsService = playersPointsService;
            this.fixtureScraperService = fixtureScraperService;
        }

        public async Task<IActionResult> Index()
        {
            var supporters = this.usersService.GetAllUsers<SupporterViewModel>();
            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();

            foreach (var supporter in supporters)
            {
                supporter.Games = supporter.Games.Where(x => x.Matchday < nextMatchday).ToList();
                foreach (var game in supporter.Games)
                {
                    var players = this.userGamesService.GetPlayersIdsByUserGameId(game.Id);
                    var points = this.playersPointsService.GetPointsByMatchdayAndPlayerIdCollection(game.Matchday, players);
                    game.Points = points;
                }
            }

            var viewModel = new CollectionOfSupporterViewModel
            {
                AllSupporters = supporters,
            };

            return this.View(viewModel);
        }
    }
}
