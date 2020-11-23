namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;

    public class SocialLinks : BaseDeletableModel<int>
    {
        public string WebsiteLink { get; set; }

        public string FacebookLink { get; set; }

        public string TweeterLink { get; set; }

        public string InstagramLink { get; set; }
    }
}
