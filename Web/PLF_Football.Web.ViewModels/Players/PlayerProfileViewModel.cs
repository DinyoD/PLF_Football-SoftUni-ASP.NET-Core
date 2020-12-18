namespace PLF_Football.Web.ViewModels.Players
{
    using System;
    using System.Collections.Generic;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.PlayersPoints;
    using PLF_Football.Web.ViewModels.SocialLinks;

    public class PlayerProfileViewModel : PlayerBasicViewModel, IMapFrom<Player>
    {
        public DateTime? DateOfBirth { get; set; }

        public string DoBAsString => this.DateOfBirth == null ? string.Empty : this.DateOfBirth?.ToString("dd.MMM.yyyy");

        public int Age { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public string ClubName { get; set; }

        public ClubSocialLinksViewModel SocialLinks { get; set; }

        public ICollection<PlayerPointsByFixtureViewModel> PlayerPoints { get; set; }
    }
}
