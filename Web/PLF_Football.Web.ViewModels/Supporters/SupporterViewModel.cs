namespace PLF_Football.Web.ViewModels.Supporters
{
    using System.Collections.Generic;
    using System.Linq;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.UserGame;

    public class SupporterViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int FavoriteTeamId { get; set; }

        public string FavoriteTeamName { get; set; }

        public int TotalPoints => this.Games.Sum(x => x.Points);

        public ICollection<GamePointsViewModel> Games { get; set; }
    }
}
