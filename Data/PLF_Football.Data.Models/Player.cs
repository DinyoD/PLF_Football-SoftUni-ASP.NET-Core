namespace PLF_Football.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using PLF_Football.Common;
    using PLF_Football.Data.Common.Models;

    public class Player : BaseDeletableModel<int>
    {
        public Player()
        {
            this.UsersGames = new HashSet<PlayersUserGames>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public string SquadNumber { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }

        public int? CountryId { get; set; }

        public Country Country { get; set; }

        public int ClubId { get; set; }

        public Club Club { get; set; }

        public string PLOverviewLink { get; set; }

        [NotMapped]
        public string PLTotalStatsLink => this.PLOverviewLink
            .Replace(GlobalConstants.PlayerOverview, GlobalConstants.PlayerTotalStats);

        // [NotMapped]
        // public string PLCurrSeasonLink => this.PLTotalStatsLink + GlobalConstants.PlayerSeasonStatsFilter;
        public int PlayerStatsId { get; set; }

        public PlayerStats PlayerStats { get; set; }

        public int SocialLinksId { get; set; }

        public SocialLinks SocialLinks { get; set; }

        [NotMapped]
        public int Price => 3_000_000;

        public virtual ICollection<PlayersUserGames> UsersGames { get; set; }
    }
}
