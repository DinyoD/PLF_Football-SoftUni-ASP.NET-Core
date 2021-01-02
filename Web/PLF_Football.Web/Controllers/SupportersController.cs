namespace PLF_Football.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Supporters;

    [Authorize]
    public class SupportersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IUserGamesService userGamesService;
        private readonly IPlayersPointsService playersPointsService;
        private readonly IFixtureScraperService fixtureScraperService;
        private readonly IClubsService clubsService;

        public SupportersController(
            IUsersService usersService,
            IUserGamesService userGamesService,
            IPlayersPointsService playersPointsService,
            IFixtureScraperService fixtureScraperService,
            IClubsService clubsService)
        {
            this.usersService = usersService;
            this.userGamesService = userGamesService;
            this.playersPointsService = playersPointsService;
            this.fixtureScraperService = fixtureScraperService;
            this.clubsService = clubsService;
        }

        public async Task<IActionResult> Index()
        {
            var supporters = await this.GetAllSupporterViewModels();
            var topFiveSupporters = supporters.OrderByDescending(x => x.TotalPoints).ThenBy(x => x.AverageTeamSum).Take(5).ToList();

            var allGames = supporters.SelectMany(x => x.Games).ToList();
            var topFiveGame = allGames.OrderByDescending(x => x.Points).Take(5);

            var viewModel = new RankingOfSupporterViewModel
            {
                TopFiveSupporters = topFiveSupporters,
                TopFiveGame = topFiveGame,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllByGamePoints(int page = 1, string matchdaySearch = "0")
        {
            var supporters = await this.GetAllSupporterViewModels();
            var allGames = supporters.SelectMany(x => x.Games).ToList();

            var viewModel = new CollectionOfSupportersGamePointsViewModel
            {
                Supporters = allGames,
                PageNumber = page,
                ItemsPerPage = GlobalConstants.SupportersPerPage,
                MatchdaySearch = int.Parse(matchdaySearch),
            };

            if (viewModel.MatchdaySearch > 0)
            {
                viewModel.Supporters = viewModel.Supporters.Where(x => x.Matchday == viewModel.MatchdaySearch).ToList();
            }

            viewModel.ItemsCount = viewModel.Supporters.Count;

            if (viewModel.PageNumber < 1)
            {
                viewModel.PageNumber = 1;
            }
            else if (viewModel.PageNumber > viewModel.PagesCount)
            {
                viewModel.PageNumber = viewModel.PagesCount;
            }

            viewModel.AllMatchdays = new List<MatchdayViewModel>() { new MatchdayViewModel { MatchdayName = "All", MatchdayNumber = 0 } };

            foreach (var matchday in allGames.Select(x => x.Matchday))
            {
                if (!viewModel.AllMatchdays.Select(x => x.MatchdayNumber).Contains(matchday))
                {
                    viewModel.AllMatchdays.Add(new MatchdayViewModel { MatchdayName = matchday.ToString(), MatchdayNumber = matchday });
                }
            }

            viewModel.AllMatchdays = viewModel.AllMatchdays.OrderBy(x => x.MatchdayNumber).ToList();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllByTotalPoints(int page = 1)
        {
            var supporters = await this.GetAllSupporterViewModels();
            var viewModel = new CollectionOfSupporterViewModel
            {
                Supporters = supporters,
                PageNumber = page,
                ItemsPerPage = GlobalConstants.SupportersPerPage,
            };
            viewModel.ItemsCount = viewModel.Supporters.Count;

            if (viewModel.PageNumber < 1)
            {
                viewModel.PageNumber = 1;
            }
            else if (viewModel.PageNumber > viewModel.PagesCount)
            {
                viewModel.PageNumber = viewModel.PagesCount;
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Club(int clubId, int page = 1)
        {
            var supporters = await this.GetAllSupporterViewModels();
            var clubSupporters = supporters.Where(x => x.FavoriteTeamId == clubId).ToList();

            var clubName = this.clubsService.GetClubNameById(clubId);
            var viewModel = new CollectionOfSuportersByClubViewModels
            {
                Supporters = clubSupporters,
                ClubName = clubName,
                ClubId = clubId,
                PageNumber = page,
                ItemsPerPage = GlobalConstants.SupportersPerPage,
            };
            viewModel.ItemsCount = viewModel.Supporters.Count;

            if (viewModel.PageNumber < 1)
            {
                viewModel.PageNumber = 1;
            }
            else if (viewModel.PageNumber > viewModel.PagesCount)
            {
                viewModel.PageNumber = viewModel.PagesCount;
            }

            return this.View(viewModel);
        }

        private async Task<ICollection<SupporterViewModel>> GetAllSupporterViewModels()
        {
            var supporters = this.usersService.GetAllUsers<SupporterViewModel>();
            var nextMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();

            foreach (var supporter in supporters)
            {
                supporter.Games = supporter.Games.Where(x => x.Matchday < nextMatchday).ToList();
                foreach (var game in supporter.Games)
                {
                    var players = this.userGamesService.GetPlayersIdsByUserGameId(game.Id);
                    if (players.Count < 11)
                    {
                        game.Points = 0;
                    }
                    else
                    {
                        var points = this.playersPointsService.GetPointsByMatchdayAndPlayerIdCollection(game.Matchday, players);
                        game.Points = points;
                    }
                }

                supporter.AverageTeamSum = supporter.Games.Count > 0 ? supporter.Games.Sum(x => x.TeamSum) / supporter.Games.Count : 0;
                supporter.PointSum = supporter.TotalPoints > 0 ? supporter.Games.Sum(x => x.TeamSum) / supporter.TotalPoints : 0;
            }

            return supporters;
        }
    }
}
