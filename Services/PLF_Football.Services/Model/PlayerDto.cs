namespace PLF_Football.Services.Model
{
    using System;

    using PLF_Football.Common;

    public class PlayerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public string SquadNumber { get; set; }

        public string PositionName { get; set; }

        public string NationalityName { get; set; }

        public string PLOverviewLink { get; set; }

        public PlayerStatsTotalDto PremierLeagueRecordTotal { get; set; }

        public PlayerSeasonStatsDto PremierLeagueSeasonRecord { get; set; }

        public SocialLinksDto SocialLinks { get; set; }

        public string PLTotalStatsLink => this.PLOverviewLink
            .Replace(GlobalConstants.PlayerOverview, GlobalConstants.PlayerTotalStats);

        public string PLCurrSeasonLink => this.PLTotalStatsLink + GlobalConstants.PlayerSeasonStatsFilter;
    }
}
