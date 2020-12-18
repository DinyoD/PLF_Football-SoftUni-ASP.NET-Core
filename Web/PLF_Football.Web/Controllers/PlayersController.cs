namespace PLF_Football.Web.Controllers
{
    using System;

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

        public IActionResult Profile(int id)
        {
            var viewModel = this.playersService.GetPlayerById<PlayerProfileViewModel>(id);
            viewModel.Age = (int)(DateTime.Now.Year - viewModel.DateOfBirth?.Year);
            if (DateTime.Now.Date < viewModel.DateOfBirth?.AddYears(viewModel.Age))
            {
                viewModel.Age--;
            }

            return this.View(viewModel);
        }

        public IActionResult Statistics(int id)
        {
            var viewModel = this.playersService.GetPlayerStatsbyId<PlayerStatsViewModel>(id);
            viewModel.ClubName = this.playersService.GetClubNameByPLayerId(id);
            return this.View(viewModel);
        }
    }
}
