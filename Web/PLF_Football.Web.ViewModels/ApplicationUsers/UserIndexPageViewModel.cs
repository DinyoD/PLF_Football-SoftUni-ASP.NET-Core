namespace PLF_Football.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;
    using System.Linq;

    using PLF_Football.Web.ViewModels.UserGame;

    public class UserIndexPageViewModel 
    {
        public string Id { get; set; }

        public int LastStartedMatchday { get; set; }

        public ICollection<UserGameTeamViewModel> Teams { get; set; }

        public int TotalPoints => this.Teams.Sum(x => x.Points);

    }
}
