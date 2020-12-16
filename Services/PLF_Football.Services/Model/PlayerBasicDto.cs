namespace PLF_Football.Services.Model
{
    using PLF_Football.Common;

    public abstract class PlayerBasicDto
    {

        public string SquadNumber { get; set; }

        public PlayerStatsDto PremierLeagueRecordTotal { get; set; }

        public string PLOverviewLink { get; set; }

        public string PLTotalStatsLink => this.PLOverviewLink
            .Replace(GlobalConstants.PlayerOverview, GlobalConstants.PlayerTotalStats);
    }
}
