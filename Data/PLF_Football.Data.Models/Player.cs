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
            this.UsersGames = new HashSet<PlayerUserGame>();
            this.PlayerPoints = new HashSet<PlayerPointsByFixture>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public string SquadNumber { get; set; }

        public int PositionId { get; set; }

        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }

        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public int ClubId { get; set; }

        [ForeignKey("ClubId")]
        public virtual Club Club { get; set; }

        public string PLOverviewLink { get; set; }

        [NotMapped]
        public string PLTotalStatsLink => this.PLOverviewLink
            .Replace(GlobalConstants.PlayerOverview, GlobalConstants.PlayerTotalStats);

        public virtual PlayerStats PlayerStats { get; set; }

        public virtual SocialLinks SocialLinks { get; set; }

        public int Price { get; set; }

        public virtual ICollection<PlayerUserGame> UsersGames { get; set; }

        public virtual ICollection<PlayerPointsByFixture> PlayerPoints { get; set; }
    }
}
