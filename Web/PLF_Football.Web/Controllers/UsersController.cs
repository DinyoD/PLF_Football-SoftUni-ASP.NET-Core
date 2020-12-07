namespace PLF_Football.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.UserGame;

    public class UsersController : BaseController
    {
        private readonly IUserGamesService userGamesService;

        public UsersController(IUserGamesService userGamesService)
        {
            this.userGamesService = userGamesService;
        }

        public IActionResult Team(string userId, int matchday)
        {
            var viewModel = this.userGamesService.GetUserGame<UserGameTeamViewModel>(userId, matchday);
            return this.View(viewModel);
        }
    }
}
