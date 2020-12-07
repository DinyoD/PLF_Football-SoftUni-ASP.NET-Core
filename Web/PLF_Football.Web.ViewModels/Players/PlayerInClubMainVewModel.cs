namespace PLF_Football.Web.ViewModels.Players
{
    using System.Globalization;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.SocialLinks;

    public class PlayerInClubMainVewModel : PlayerBasicViewModel, IMapFrom<Player>
    {
        public virtual ClubSocialLinksViewModel SocialLinks { get; set; }
    }
}
