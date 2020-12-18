namespace PLF_Football.Web.ViewModels.Players
{
    using System.Collections.Generic;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.PlayersPoints;

    public class UserTeamPlayerViewModel : PlayerBasicViewModel, IMapFrom<Player>
    {
        public ICollection<PlayerPointsByFixtureViewModel> PlayerPoints { get; set; }
    }
}
