namespace PLF_Football.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly IClubsService clubService;

        public HomeController(IClubsService clubService)
        {
            this.clubService = clubService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
