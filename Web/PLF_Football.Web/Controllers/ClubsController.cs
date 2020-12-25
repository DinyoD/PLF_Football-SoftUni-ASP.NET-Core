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

        public ClubsController(
            IClubsService clubService)
        {
            this.clubService = clubService;
        }

        public IActionResult Index(int id)
        {
            var viewModel = this.clubService.GetById<ClubMainVewModel>(id);
            return this.View(viewModel);
        }
    }
}
