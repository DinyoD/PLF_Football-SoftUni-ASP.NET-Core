namespace PLF_Football.Web.Controllers
{
    using PLF_Football.Services.Data;

    public class FixturesController : BaseController
    {
        private readonly IFixtureService fixtureService;

        public FixturesController(IFixtureService fixtureService)
        {
            this.fixtureService = fixtureService;
        }
    }
}
