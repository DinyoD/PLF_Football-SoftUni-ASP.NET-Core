namespace PLF_Football.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using PLF_Football.Common;
    using PLF_Football.Data.Common.Models;

    public class Club : BaseModel<int>
    {
        public Club()
        {
            this.Players = new HashSet<Player>();
            this.HomeMatches = new HashSet<Fixture>();
            this.AwayMatches = new HashSet<Fixture>();
            this.Supporters = new HashSet<ApplicationUser>();
        }

        public string PLLink { get; set; }

        [NotMapped]
        public string PLSquadLink => this.PLLink
            .Replace(GlobalConstants.ClubOverview, GlobalConstants.ClubSquad);

        // [NotMapped]
        // public string PLStadiumLink => this.PLLink
        //     .Replace(GlobalConstants.ClubOverview, GlobalConstants.ClubStadium);
        public string Name { get; set; }

        public string BadgeUrl { get; set; }

        public virtual Stadium Stadium { get; set; }

        public virtual SocialLinks SocialLinks { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Fixture> HomeMatches { get; set; }

        public virtual ICollection<Fixture> AwayMatches { get; set; }

        public virtual ICollection<ApplicationUser> Supporters { get; set; }
    }
}
