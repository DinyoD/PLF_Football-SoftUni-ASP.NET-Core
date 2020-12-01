namespace PLF_Football.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;
    using PLF_Football.Common;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Model;

    public class PLClubsScraperService : IPLClubsScraperService
    {
        private readonly IBrowsingContext context;
        private readonly IRepository<Club> clubRepo;
        private readonly IDeletableEntityRepository<SocialLinks> sociallinksRepo;
        private readonly IRepository<Stadium> stadiumRepo;
        private readonly IRepository<Position> positionRepo;
        private readonly IRepository<Country> countryRepo;

        public PLClubsScraperService(
            IRepository<Club> clubRepo,
            IDeletableEntityRepository<SocialLinks> sociallinksRepo,
            IRepository<Stadium> stadiumRepo,
            IRepository<Position> positionRepo,
            IRepository<Country> countryRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.clubRepo = clubRepo;
            this.sociallinksRepo = sociallinksRepo;
            this.stadiumRepo = stadiumRepo;
            this.positionRepo = positionRepo;
            this.countryRepo = countryRepo;
        }

        public async Task ImportClubsAsync()
        {
            var clubsDto = await this.GetClubsInfoAsync();
            foreach (var clubDto in clubsDto)
            {
                Club club = this.CreateClub(clubDto);

                await this.clubRepo.AddAsync(club);
            }
        }

        private Club CreateClub(ClubDto clubDto)
        {
            var club = new Club
            {
                Name = clubDto.Name,
                BadgeUrl = clubDto.BadgeUrl,
                PLLink = clubDto.PLLink,
                Stadium = new Stadium
                {
                    Name = clubDto.Stadium.Name,
                },
                SocialLinks = new SocialLinks
                {
                    WebsiteLink = clubDto.SocialLinks.WebsiteLink,
                    FacebookLink = clubDto.SocialLinks.FacebookLink,
                    TweeterLink = clubDto.SocialLinks.TweeterLink,
                    InstagramLink = clubDto.SocialLinks.InstagramLink,
                },
            };

            return club;
        }

        private async Task<ConcurrentBag<ClubDto>> GetClubsInfoAsync()
        {
            var document = await this.context.OpenAsync(GlobalConstants.PremierLeagueLink);

            var clubsPages = document.QuerySelectorAll(".indexItem").ToList();

            var clubsDtosList = new ConcurrentBag<ClubDto>();

            foreach (var clubPage in clubsPages)
            {
                clubsDtosList.Add(this.GetClubDto(clubPage));
            }

            foreach (var clubDto in clubsDtosList)
            {
                clubDto.SocialLinks = await this.GetSocialLinksDto(clubDto.PLStadiumLink);
            }

            return clubsDtosList;
        }

        private ClubDto GetClubDto(AngleSharp.Dom.IElement clubPage)
        {
            var clubDto = new ClubDto();

            clubDto.Name = clubPage.QuerySelector(".clubName").TextContent;

            clubDto.BadgeUrl = clubPage.QuerySelector(".badge-image").GetAttribute("src");

            var clubHref = clubPage.GetAttribute("href");

            clubDto.PLLink = GlobalConstants.PremierLeagueLink
                .Replace("/clubs", string.Empty) + clubHref;

            clubDto.Stadium = this.GetStadiumDto(clubPage);

            return clubDto;
        }

        private async Task<SocialLinksDto> GetSocialLinksDto(string clubStadiumLink)
        {
            var clubStadiumPage = await this.context.OpenAsync(clubStadiumLink);

            var socialLinkDto = new SocialLinksDto
            {
                WebsiteLink = clubStadiumPage.QuerySelector(".website > a").GetAttribute("href"),
                FacebookLink = clubStadiumPage.QuerySelector(".facebook-option").GetAttribute("href"),
                TweeterLink = clubStadiumPage.QuerySelector(".twitter-option").GetAttribute("href"),
                InstagramLink = clubStadiumPage.QuerySelector(".instagram-option").GetAttribute("href"),
            };
            return socialLinkDto;
        }

        private StadiumDto GetStadiumDto(AngleSharp.Dom.IElement clubPage)
        {
            var stadiumDto = new StadiumDto
            {
                Name = clubPage.QuerySelector(".stadiumName").TextContent,

                // TODO Stadium Info, Stadium image
            };
            return stadiumDto;
        }
    }
}
