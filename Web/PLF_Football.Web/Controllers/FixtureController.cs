namespace PLF_Football.Web.Controllers
{
    using PLF_Football.Services.Data;

    public class FixtureController : BaseController
    {
        private readonly IFixtureService fixtureService;

        public FixtureController(IFixtureService fixtureService)
        {
            this.fixtureService = fixtureService;
        }


    }
}
