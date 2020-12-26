namespace PLF_Football.Web.ViewModels.ViewComponents
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using PLF_Football.Common;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Players;

    public class UserActiveTeamViewModel
    {
        public string UserId { get; set; }

        public int Matchday { get; set; }

        public int Budget { get; set; }

        public string BudgetAsString =>
            this.Budget.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));

        public int PlayersInTeam { get; set; }

        public virtual List<PlayerForUserActiveTeamViewModel> MatchdayTeam { get; set; }

    }
}
