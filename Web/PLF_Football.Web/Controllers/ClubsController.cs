namespace PLF_Football.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Clubs;
    using PLF_Football.Web.ViewModels.Fixtures;

    public class ClubsController : BaseController
    {
        private readonly IClubsService clubService;
        private readonly IFixtureService fixtiresService;

        public ClubsController(
            IClubsService clubService,
            IFixtureService fixtiresService)
        {
            this.clubService = clubService;
            this.fixtiresService = fixtiresService;
        }

        public IActionResult Index(int id)
        {
            var viewModel = this.clubService.GetById<ClubMainVewModel>(id);
            return this.View(viewModel);
        }

        public IActionResult Fixtures(int teamId)
        {
            var fixtures = this.fixtiresService
                .GetFixturesOnAndBeforeSpecificMatchdayByClub<FixtureBasicViewModel>(GlobalConstants.AllFixtureCount, teamId);

            var viewModel = new CollectionOfFixturesViewModel
            {
                AllFixtures = fixtures,
            };

            return this.View(viewModel);
        }
    }
}
