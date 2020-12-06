namespace PLF_Football.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Players;

    public class PlayersController : BaseController
    {
        private readonly IPlayersService playersService;

        public PlayersController(IPlayersService playersService)
        {
            this.playersService = playersService;
        }

        public IActionResult Statistics(int id)
        {
            var viewModel = this.playersService.GetPlayerStatsbyId<PlayerStatsViewModel>(id);
            return this.View(viewModel);
        }
    }
}
