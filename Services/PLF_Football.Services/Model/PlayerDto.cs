namespace PLF_Football.Services.Model
{
    using System;

    using PLF_Football.Common;

    public class PlayerDto : PlayerBasicDto
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public string PositionName { get; set; }

        public string CountryName { get; set; }

        public SocialLinksDto SocialLinks { get; set; }
    }
}
