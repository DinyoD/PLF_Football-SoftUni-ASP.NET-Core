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
                ItemsPerPage = GlobalConstants.PlayersPerPage,
                CurrentFilter = currentFilter,
                SortOrder = sortOrder,
                PageNumber = page,
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                page = 1;
                viewModel.AllPlayers = this.playerService.GetPlayersBySearchingString<PlayerAdminViewModel>(searchString);
                viewModel.SearchString = searchString;
            }
            else
            {
                viewModel.AllPlayers = this.playerService.GetAll<PlayerAdminViewModel>();
            }

            viewModel.ItemsCount = viewModel.AllPlayers.Count();

            if (viewModel.PageNumber < 1)
            {
                viewModel.PageNumber = 1;
            }
            else if (viewModel.PageNumber > viewModel.PagesCount)
            {
                viewModel.PageNumber = viewModel.PagesCount;
            }

            var newSortOrder = string.Empty;

            if (viewModel.PageNumber == 1)
            {
                switch (sortOrder)
                {
                    case "name":
                        newSortOrder = viewModel.CurrentFilter == "name" ? newSortOrder = "name_desc" : newSortOrder = "name";
                        break;
                    case "price":
                        newSortOrder = viewModel.CurrentFilter == "price" ? newSortOrder = "price_desc" : newSortOrder = "price";
                        break;
                    case "clubName":
                        newSortOrder = viewModel.CurrentFilter == "clubName" ? newSortOrder = "clubName_desc" : newSortOrder = "clubName";
                        break;
                    default:
                        newSortOrder = viewModel.CurrentFilter;
                        break;
                }

                viewModel.SortOrder = newSortOrder;
                viewModel.CurrentFilter = newSortOrder;
            }

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
