namespace PLF_Football.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.ApplicationUsers;
    using PLF_Football.Web.ViewModels.UserGame;

    public class UsersController : BaseController
    {
        private readonly IUserGamesService userGamesService;
        private readonly IUsersService usersService;

        public UsersController(
            IUserGamesService userGamesService,
            IUsersService usersService)
        {
            this.userGamesService = userGamesService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = this.usersService.GetUserById<UserProfileViewModel>(userId);
            return this.View(viewModel);
        }

        public IActionResult Team(string userId, int matchday)
        {
            var viewModel = this.userGamesService.GetUserGame<UserGameTeamViewModel>(userId, matchday);
            return this.View(viewModel);
        }
    }
}
