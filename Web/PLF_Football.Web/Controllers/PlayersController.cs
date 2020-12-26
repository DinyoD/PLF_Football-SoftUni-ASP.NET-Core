namespace PLF_Football.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Clubs;
    using PLF_Football.Web.ViewModels.Players;
    using PLF_Football.Web.ViewModels.PlayersPoints;
    using PLF_Football.Web.ViewModels.Position;

    [Authorize]
    public class PlayersController : BaseController
    {
        private readonly IPlayersService playersService;
        private readonly IClubsService clubsService;
        private readonly IPositionService positionService;
        private readonly IPlayersPointsService playersPointsService;
        private readonly IFixtureScraperService fixtureScraperService;

        public PlayersController(
            IPlayersService playersService,
            IClubsService clubsService,
            IPositionService positionService,
            IPlayersPointsService playersPointsService,
            IFixtureScraperService fixtureScraperService)
        {
            this.playersService = playersService;
            this.clubsService = clubsService;
            this.positionService = positionService;
            this.playersPointsService = playersPointsService;
            this.fixtureScraperService = fixtureScraperService;
        }

        public IActionResult Profile(int id)
        {
            var viewModel = this.playersService.GetPlayerById<PlayerProfileViewModel>(id);
            viewModel.Age = (int)(DateTime.Now.Year - viewModel.DateOfBirth?.Year);
            if (DateTime.Now.Date < viewModel.DateOfBirth?.AddYears(viewModel.Age))
            {
                viewModel.Age--;
            }

            return this.View(viewModel);
        }

        public IActionResult Statistics(int id)
        {
            var viewModel = this.playersService.GetPlayerStatsbyId<PlayerStatsViewModel>(id);
            viewModel.ClubName = this.playersService.GetClubNameByPLayerId(id);
            return this.View(viewModel);
        }

        public IActionResult All(int clubId, int positionId, string searchString, int page = 1)
        {
            var viewModel = new AllPlayersCollectionViewModel
            {
                PositionSearch = positionId,
                ClubSearch = clubId,
                SearchString = searchString,
                PageNumber = page,
                ItemsPerPage = GlobalConstants.PlayersPerPage,
                AllPlayers = this.playersService.GetAll<PlayerForAllPlayersViewModel>(),
                Positions = this.positionService.GetAll<PositionBasicViewModel>(),
                Clubs = this.clubsService.GetAll<ClubBasicViewModel>(),
            };

            viewModel.ItemsCount = viewModel.AllPlayers.Count;
            viewModel.Positions.Add(new PositionBasicViewModel { Name = "All positions", Id = -1 });
            viewModel.Positions = viewModel.Positions.OrderBy(x => x.Id).ToList();
            viewModel.Clubs.Add(new ClubBasicViewModel { Name = "All clubs", Id = -1 });
            viewModel.Clubs = viewModel.Clubs.OrderBy(x => x.Id).ToList();

            if (viewModel.PageNumber < 1)
            {
                viewModel.PageNumber = 1;
            }
            else if (viewModel.PageNumber > viewModel.PagesCount)
            {
                viewModel.PageNumber = viewModel.PagesCount;
            }

            if (clubId > 0)
            {
                viewModel.AllPlayers = viewModel.AllPlayers.Where(x => x.Club.Id == clubId).ToList();
            }

            if (positionId > 0)
            {
                viewModel.AllPlayers = viewModel.AllPlayers.Where(x => x.Position.Id == positionId).ToList();
            }

            if (searchString != null)
            {
                viewModel.AllPlayers = viewModel.AllPlayers.Where(x => x.FullName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Points(int id)
        {
            var pointsByFixture = this.playersPointsService.GetAllPointsByMatchdaysForPlayer<PointsByFixtureViewModel>(id);
            var lastMatchday = await this.fixtureScraperService.GetFirstNotStartedMatchdayAsync();
            for (int i = 13; i < lastMatchday; i++)
            {
                if (!pointsByFixture.Any(x => x.Matchday == i))
                {
                    pointsByFixture.Add(new PointsByFixtureViewModel { Matchday = i, Points = 0 });
                }
            }

            var clubName = this.playersService.GetClubNameByPLayerId(id);
            var playerFullName = this.playersService.GetPlayerFullNameById(id);
            var viewModel = new PlayerPointsViewModel
            {
                PlayerId = id,
                PlayerName = playerFullName,
                ClubName = clubName,
                PointsByFixture = pointsByFixture.OrderBy(x => x.Matchday).ToList(),
            };

            return this.View(viewModel);
        }
    }
}
