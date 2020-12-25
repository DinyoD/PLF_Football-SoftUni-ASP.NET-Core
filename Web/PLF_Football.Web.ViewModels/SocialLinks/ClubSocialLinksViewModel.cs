namespace PLF_Football.Web.ViewModels.SocialLinks
{
    using System.Text.RegularExpressions;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class ClubSocialLinksViewModel : IMapFrom<SocialLinks>
    {
        public string WebsiteLink { get; set; }

        public string WebsiteName => Regex.Match(this.WebsiteLink, @"www.[a-z]+.[a-z]{2,3}[.]?[a-z]{0,2}").Value;

        public string FacebookLink { get; set; }

        public string TweeterLink { get; set; }

        public string InstagramLink { get; set; }
    }
}
