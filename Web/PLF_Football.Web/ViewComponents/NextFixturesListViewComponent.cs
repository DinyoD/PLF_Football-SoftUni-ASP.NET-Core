namespace PLF_Football.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.ViewComponents;

    public class NextFixturesListViewComponent : ViewComponent
    {
        private readonly IFixtureService fixtureService;
        private readonly IFixtureScraperService fixtureScraperService;

        public NextFixturesListViewComponent(
            IFixtureService fixtureService,
            IFixtureScraperService fixtureScraperService)
        {
            this.fixtureService = fixtureService;
            this.fixtureScraperService = fixtureScraperService;
        }

        public IViewComponentResult Invoke()
        {
            var nextMatchday = this.fixtureScraperService.GetFirstNotStartedMatchdayAsync().GetAwaiter().GetResult();
            var fixtures = this.fixtureService
                                .GetFixturesAfterSpecificAndBeforeOrOnNextMatchday<FixtureViewModel>(
                                                                                        nextMatchday,
                                                                                        nextMatchday);
            var viewModel = new NextFixturesListViewModel
            {
                Matchday = nextMatchday,
                Fixture = fixtures,
            };

            return this.View(viewModel);
        }
    }
}
