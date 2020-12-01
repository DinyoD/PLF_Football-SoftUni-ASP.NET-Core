namespace PLF_Football.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services.Data.Models;
    using PLF_Football.Web.ViewModels.ViewComponents;

    public class ClubsLinksViewComponent : ViewComponent
    {
        private readonly IClubsService clubService;

        public ClubsLinksViewComponent(IClubsService clubService)
        {
            this.clubService = clubService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new ClubLinksInListViewModel
            {
                Clubs = this.clubService.GetAll<ClubsLinksViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
