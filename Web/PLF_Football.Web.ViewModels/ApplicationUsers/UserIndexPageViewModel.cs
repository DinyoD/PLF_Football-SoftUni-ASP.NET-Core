namespace PLF_Football.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;
    using System.Linq;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Fixtures;
    using PLF_Football.Web.ViewModels.UserGame;

    public class UserIndexPageViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public int FavoriteTeamId { get; set; }

        public string FavoriteTeamName { get; set; }

        public int LastStartedMatchday { get; set; }

        public ICollection<UserGameTeamViewModel> Teams { get; set; }

        public ICollection<FixtureBasicViewModel> Fixtures { get; set; }

        public int TotalPoints => this.Teams.Sum(x => x.Points);
    }
}
