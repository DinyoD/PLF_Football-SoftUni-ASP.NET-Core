namespace PLF_Football.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Common;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.Players;

    [Area("Administration")]
    public class PlayersController : AdministrationController
    {
        private readonly IPlayersService playerService;

        public PlayersController(IPlayersService playerService)
        {
            this.playerService = playerService;
        }

        // GET: Administration/Players
        public IActionResult Index(string searchString, string sortOrder, string currentFilter, int page = 1)
        {
            var viewModel = new AllPlayersCollectionAdminViewModel
            {
                AllPlayers = this.playerService.GetAll<PlayerAdminViewModel>(),
                ItemsCount = this.playerService.GetCount(),
                ItemsPerPage = GlobalConstants.PlayersPerPage,
                PageNumber = page,
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                viewModel.PageNumber = 1;
                viewModel.AllPlayers = viewModel.AllPlayers.Where(s => s.FullNameSequence.Contains(searchString)
                                                                        || s.ClubName.ToLower().Contains(searchString)).ToList();
            }

            var newSortOrder = string.Empty;

            switch (sortOrder)
            {
                case "name":
                    newSortOrder = currentFilter == "name" ? newSortOrder = "name_desc" : newSortOrder = "name";
                    break;
                case "price":
                    newSortOrder = currentFilter == "price" ? newSortOrder = "price_desc" : newSortOrder = "price";
                    break;
                case "clubName":
                    newSortOrder = currentFilter == "clubName" ? newSortOrder = "clubName_desc" : newSortOrder = "clubName";
                    break;
                default:
                    newSortOrder = currentFilter;
                    break;
            }

            viewModel.SortOrder = newSortOrder;

            viewModel.AllPlayers = viewModel.SortOrder switch
            {
                "name_desc" => viewModel.AllPlayers.OrderByDescending(s => s.LastName).ToList(),
                "price" => viewModel.AllPlayers.OrderBy(s => s.Price).ToList(),
                "price_desc" => viewModel.AllPlayers.OrderByDescending(s => s.Price).ToList(),
                "clubName" => viewModel.AllPlayers.OrderBy(s => s.ClubName).ToList(),
                "clubName_desc" => viewModel.AllPlayers.OrderByDescending(s => s.ClubName).ToList(),
                _ => viewModel.AllPlayers.OrderBy(s => s.LastName).ToList(),
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Index(
            string searchString,
            AllPlayersCollectionAdminViewModel viewModel)
        {
            if (searchString != null)
            {
                viewModel.PageNumber = 1;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                viewModel.AllPlayers = viewModel.AllPlayers.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString)).ToList();
            }

            return this.View(viewModel);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var playerDto = this.playerService.GetPlayerById<PlayerAdminViewModel>(id);
            if (playerDto == null)
            {
                return this.NotFound();
            }

            return this.View(playerDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id, PlayerAdminViewModel player)
        {
            if (id != player.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                await this.playerService.UpdatePlayerPrice(id, player.Price);
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(player);
        }
    }
}
