namespace PLF_Football.Web.ViewModels.Clubs
{
    using System.Collections.Generic;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.ApplicationUsers;
    using PLF_Football.Web.ViewModels.Fixtures;
    using PLF_Football.Web.ViewModels.Players;
    using PLF_Football.Web.ViewModels.SocialLinks;

    public class ClubMainVewModel : IMapFrom<Club>
    {
        public int Id { get; set; }

        public string PLLink { get; set; }

        public string Name { get; set; }

        public string BadgeUrl { get; set; }

        public string StadiumName { get; set; }

        public virtual ClubSocialLinksViewModel SocialLinks { get; set; }

        public virtual ICollection<PlayerInClubMainVewModel> Players { get; set; }

        public virtual ICollection<FixtureInClubMainViewModel> HomeMatches { get; set; }

        public virtual ICollection<FixtureInClubMainViewModel> AwayMatches { get; set; }

        public virtual ICollection<SupporterViewModel> Supporters { get; set; }
    }
}
