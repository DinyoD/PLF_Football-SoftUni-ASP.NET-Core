namespace PLF_Football.Web.ViewModels.SocialLinks
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class ClubSocialLinksViewModel : IMapFrom<SocialLinks>
    {
        public string WebsiteLink { get; set; }

        public string FacebookLink { get; set; }

        public string TweeterLink { get; set; }

        public string InstagramLink { get; set; }
    }
}
