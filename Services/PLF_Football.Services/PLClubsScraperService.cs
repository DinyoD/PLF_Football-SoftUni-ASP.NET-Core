namespace PLF_Football.Services
{
    using System.Collections.Concurrent;
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
        private readonly IDeletableEntityRepository<Club> clubRepo;
        private readonly IDeletableEntityRepository<SocialLinks> sociallinksRepo;
        private readonly IDeletableEntityRepository<Stadium> stadiumRepo;

        public PLClubsScraperService(
            IDeletableEntityRepository<Club> clubRepo,
            IDeletableEntityRepository<SocialLinks> sociallinksRepo,
            IDeletableEntityRepository<Stadium> stadiumRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.clubRepo = clubRepo;
            this.sociallinksRepo = sociallinksRepo;
            this.stadiumRepo = stadiumRepo;
        }

        public async Task ImportClubsInfoAsync()
        {
            var clubsDto = await this.GetClubsInfoAsync();
            foreach (var dto in clubsDto)
            {
                var club = new Club
                {
                    Name = dto.Name,
                    BadgeUrl = dto.BadgeUrl,
                    PLLink = dto.PLLink,
                    Stadium = new Stadium
                    {
                        Name = dto.Stadium.Name,
                    },
                    SocialLinks = new SocialLinks
                    {
                        WebsiteLink = dto.SocialLinks.WebsiteLink,
                        FacebookLink = dto.SocialLinks.FacebookLink,
                        TweeterLink = dto.SocialLinks.TweeterLink,
                        InstagramLink = dto.SocialLinks.InstagramLink,
                    },
                };

                await this.clubRepo.AddAsync(club);
                await this.clubRepo.SaveChangesAsync();
            }
        }

        private async Task<ConcurrentBag<ClubDto>> GetClubsInfoAsync()
        {
            var document = await this.context.OpenAsync(GlobalConstants.PremierLeagueLink);

            var clubs = document.QuerySelectorAll(".indexItem").ToList();

            var clubsInfo = new ConcurrentBag<ClubDto>();

            foreach (var club in clubs)
            {
                clubsInfo.Add(this.GetClubDto(club));
            }

            foreach (var club in clubsInfo)
            {
                club.SocialLinks = await this.AddSocialLinksDtoToClubDto(club);
            }

            return clubsInfo;
        }

        private ClubDto GetClubDto(AngleSharp.Dom.IElement club)
        {
            var clubInfo = new ClubDto();

            clubInfo.Name = club.QuerySelector(".clubName").TextContent;

            clubInfo.BadgeUrl = club.QuerySelector(".badge-image").GetAttribute("src");

            var clubHref = club.GetAttribute("href");

            clubInfo.PLLink = GlobalConstants.PremierLeagueLink
                .Replace("/clubs", string.Empty) + clubHref;

            clubInfo.Stadium = this.GetStadiumDto(club);

            return clubInfo;
        }

        private async Task<SocialLinksDto> AddSocialLinksDtoToClubDto(ClubDto club)
        {
            var clubStadiumPage = await this.context.OpenAsync(club.PLStadiumLink);

            var socialLinkDto = new SocialLinksDto
            {
                WebsiteLink = clubStadiumPage.QuerySelector(".website > a").GetAttribute("href"),
                FacebookLink = clubStadiumPage.QuerySelector(".facebook-option").GetAttribute("href"),
                TweeterLink = clubStadiumPage.QuerySelector(".twitter-option").GetAttribute("href"),
                InstagramLink = clubStadiumPage.QuerySelector(".instagram-option").GetAttribute("href"),
            };
            return socialLinkDto;
        }

        private StadiumDto GetStadiumDto(AngleSharp.Dom.IElement club)
        {
            var stadiumDto = new StadiumDto
            {
                Name = club.QuerySelector(".stadiumName").TextContent,

                // TODO Stadium Info, Stadium image
            };
            return stadiumDto;
        }
    }
}
