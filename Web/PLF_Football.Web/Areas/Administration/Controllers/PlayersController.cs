namespace PLF_Football.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Web.Areas.Administration.Services;
    using PLF_Football.Web.ViewModels.Players;

    [Area("Administration")]
    public class PlayersController : AdministrationController
    {
        private readonly IPlayerAdminService playerService;

        public PlayersController(IPlayerAdminService playerService)
        {
            this.playerService = playerService;
        }

        // GET: Administration/Players
        public IActionResult Index()
        {
            var playersDtosCollection = this.playerService.GetAll<PlayerAdminViewModel>();
            var viewModel = new AllPlayersCollectionAdminViewModel() { AllPlayers = playersDtosCollection };
            return this.View(viewModel);
        }

        // GET: Administration/Players/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var playerDto = this.playerService.GetPlayerbyId<PlayerAdminViewModel>(id);
            if (playerDto == null)
            {
                return this.NotFound();
            }

            return this.View(playerDto);
        }

        // POST: Administration/Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
