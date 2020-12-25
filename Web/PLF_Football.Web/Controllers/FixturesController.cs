namespace PLF_Football.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Fixtures;

    [Authorize]
    public class FixturesController : BaseController
    {
        private readonly IFixtureService fixtureService;
        private readonly IUsersService usersService;

        public FixturesController(
            IFixtureService fixtureService,
            IUsersService usersService)
        {
            this.fixtureService = fixtureService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var allFixtures = this.fixtureService.GetAllFixtures<FixtureBasicViewModel>();
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userteamId = this.usersService.GetFavoriteClubIdByUserId(userId);
            var viewModel = new CollectionOfFixturesViewModel
            {
                UserTeamId = userteamId,
                AllFixtures = allFixtures,
            };
            return this.View(viewModel);
        }

        public IActionResult Club(int clubId)
        {
            var fixtures = this.fixtureService
                .GetFixturesOnAndBeforeSpecificMatchdayByClub<FixtureBasicViewModel>(GlobalConstants.AllFixtureCount, clubId);

            var viewModel = new CollectionOfFixturesViewModel
            {
                AllFixtures = fixtures,
            };

            return this.View(viewModel);
        }
    }
}
