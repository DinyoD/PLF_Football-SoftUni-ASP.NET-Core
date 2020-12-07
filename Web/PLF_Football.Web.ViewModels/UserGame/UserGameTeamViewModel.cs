namespace PLF_Football.Web.ViewModels.UserGame
{
    using System.Collections.Generic;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Players;

    public class UserGameTeamViewModel : IMapFrom<UserGame>
    {
        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public string UserFavoriteTeamName { get; set; }

        public int UserTotalPoints { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }

        public virtual ICollection<UserTeamPlayerViewModel> MatchdayTeam { get; set; }
    }
}
