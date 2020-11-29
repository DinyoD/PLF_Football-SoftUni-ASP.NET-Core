namespace PLF_Football.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using PLF_Football.Data.Common.Models;

    public class SocialLinks : BaseDeletableModel<int>
    {
        public int? ClubId { get; set; }

        [ForeignKey("ClubId")]
        public Club Club { get; set; }

        public int? PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        public string WebsiteLink { get; set; }

        public string FacebookLink { get; set; }

        public string TweeterLink { get; set; }

        public string InstagramLink { get; set; }
    }
}
