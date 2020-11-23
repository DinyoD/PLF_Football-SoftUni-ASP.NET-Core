namespace PLF_Football.Services.Model
{
    using PLF_Football.Common;

    public class ClubDto
    {
        public string PLLink { get; set; }

        public string PLSquadLink => this.PLLink
            .Replace(GlobalConstants.ClubOverview, GlobalConstants.ClubSquad);

        public string PLStadiumLink => this.PLLink
            .Replace(GlobalConstants.ClubOverview, GlobalConstants.ClubStadium);

        public string Name { get; set; }

        public string BadgeUrl { get; set; }

        public int StadiumId { get; set; }

        public StadiumDto Stadium { get; set; }

        public SocialLinksDto SocialLinks { get; set; }
    }
}
