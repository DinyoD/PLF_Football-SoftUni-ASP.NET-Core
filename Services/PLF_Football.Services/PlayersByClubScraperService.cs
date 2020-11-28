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

    public class PlayersByClubScraperService : IPlayersByClubScraperService
    {
        private readonly IBrowsingContext context;
        private readonly IRepository<Club> clubRepo;
        private readonly IDeletableEntityRepository<Player> playersRepo;
        private readonly IRepository<Position> positionRepo;
        private readonly IRepository<Country> countryRepo;

        public PlayersByClubScraperService(
            IRepository<Club> clubRepo,
            IDeletableEntityRepository<Player> playersRepo,
            IRepository<Position> positionRepo,
            IRepository<Country> countryRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.clubRepo = clubRepo;
            this.playersRepo = playersRepo;
            this.positionRepo = positionRepo;
            this.countryRepo = countryRepo;
        }

        public async Task ImportPlayersInfoAsync(Club club)
        {
            var players = await this.GetClubPlayers(club);

            club.Players = players;
        }

        private async Task<ICollection<Player>> GetClubPlayers(Club club)
        {
            var playersDtoByClub = await this.GetPlayersDtoByClub(club);
            var players = new List<Player>();

            foreach (var playerDto in playersDtoByClub)
            {
                var player = this.CreatePlayerFromPlayerDto(playerDto, club.Id);

                player.PositionId = this.positionRepo
                                    .All()
                                    .FirstOrDefault(x => x.Name == playerDto.PositionName).Id;
                player.CountryId = this.countryRepo
                                    .All()
                                    .FirstOrDefault(x => x.Name == playerDto.CountryName)?.Id;

                players.Add(player);
            }

            return players;
        }

        private async Task<ConcurrentBag<PlayerDto>> GetPlayersDtoByClub(Club club)
        {
            var clubSquadPage = await this.context.OpenAsync(club.PLSquadLink);

            var players = clubSquadPage.QuerySelectorAll(".squadList > li > a");

            var playersDtoByClub = new ConcurrentBag<PlayerDto>();

            foreach (var item in players)
            {
                var playerDto = await this.ScrapePlayersInfoAsync(item);
                playersDtoByClub.Add(playerDto);
            }

            return playersDtoByClub;
        }

        private async Task<PlayerDto> ScrapePlayersInfoAsync(AngleSharp.Dom.IElement item)
        {
            var playerDto = new PlayerDto();

            playerDto.PLOverviewLink = GlobalConstants.PremierLeagueLink
                .Replace("/clubs", string.Empty) + item.GetAttribute("href");

            playerDto.PositionName = item.QuerySelector(".position")?.TextContent;

            var playerData = item.QuerySelector(".squadPlayerHeader > img").GetAttribute("data-player");
            playerDto.ImageUrl = GlobalConstants.PlayerImgLink + playerData + ".png";

            var playerPage = await this.context.OpenAsync(playerDto.PLOverviewLink);

            playerDto.SquadNumber = playerPage.QuerySelector(".playerDetails > .number")?.TextContent;

            var fullName = playerPage.QuerySelector(".playerDetails  .name")?.TextContent;

            var names = fullName?.Split(" ");
            if (names != null && names.Length > 1)
            {
                playerDto.FirstName = names[0];
                playerDto.LastName = names[1];
            }
            else
            {
                playerDto.LastName = fullName;
            }

            playerDto.CountryName = playerPage.QuerySelector(".playerCountry")?.TextContent.Trim();

            var playerDateOfBirth =
                playerPage.QuerySelector(".pdcol2  div.info")?.TextContent.Trim().Split(" ").FirstOrDefault();

            playerDto.DateOfBirth = playerDateOfBirth != null
                ? DateTime.ParseExact(playerDateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                : (DateTime?)null;

            var playerBodyStats = playerPage.QuerySelectorAll(".pdcol3  .info");

            if (playerBodyStats.Length > 1)
            {
                playerDto.Height = int.Parse(playerBodyStats[0]?.TextContent.Replace("cm", string.Empty));
                playerDto.Weight = int.Parse(playerBodyStats[1]?.TextContent.Replace("kg", string.Empty));
            }
            else if (playerBodyStats.Length == 1)
            {
                playerDto.Height = int.Parse(playerBodyStats[0]?.TextContent.Replace("cm", string.Empty));
            }

            playerDto.PremierLeagueRecordTotal = await this.GetPlayerStatsAsync(playerDto);
            playerDto.SocialLinks = await this.GetPlayerSocialLinksAsync(playerDto);

            return playerDto;
        }

        private async Task<SocialLinksDto> GetPlayerSocialLinksAsync(PlayerDto playerDto)
        {
            var playerStatsPage = await this.context.OpenAsync(playerDto.PLTotalStatsLink);

            var socilaLinksDto = new SocialLinksDto
            {
                FacebookLink = playerStatsPage.QuerySelector(".facebook-option")?.GetAttribute("href"),
                TweeterLink = playerStatsPage.QuerySelector(".twitter-option")?.GetAttribute("href"),
                InstagramLink = playerStatsPage.QuerySelector(".instagram-option")?.GetAttribute("href"),
            };

            return socilaLinksDto;
        }

        private async Task<PlayerStatsDto> GetPlayerStatsAsync(PlayerDto playerDto)
        {
            var playerStatsPage = await this.context.OpenAsync(playerDto.PLTotalStatsLink);

            var cleanSheets = playerStatsPage.QuerySelector(".statclean_sheet")?.TextContent.Trim().Replace(",", string.Empty);
            var goalsConc = playerStatsPage.QuerySelector(".statgoals_conceded")?.TextContent.Trim().Replace(",", string.Empty);
            var fouls = playerStatsPage.QuerySelector(".statfouls")?.TextContent.Trim().Replace(",", string.Empty);
            var shots = playerStatsPage.QuerySelector(".stattotal_scoring_att")?.TextContent.Trim().Replace(",", string.Empty);
            var shotsOnTarget =
                playerStatsPage.QuerySelector(".statontarget_scoring_att")?.TextContent.Trim().Replace(",", string.Empty);
            var passes = playerStatsPage.QuerySelector(".stattotal_pass")?.TextContent.Trim().Replace(",", string.Empty);
            var bigChanceCreated =
                playerStatsPage.QuerySelector(".statbig_chance_created")?.TextContent.Trim().Replace(",", string.Empty);
            var tackles = playerStatsPage.QuerySelector(".stattotal_tackle")?.TextContent.Trim().Replace(",", string.Empty);
            var cleareances = playerStatsPage.QuerySelector(".stattotal_clearance")?.TextContent.Trim().Replace(",", string.Empty);

            var playerStatsTotalDto = new PlayerStatsDto
            {
                Appearances = int.Parse(playerStatsPage.QuerySelector(".statappearances")?.TextContent.Trim()),
                Wins = int.Parse(playerStatsPage.QuerySelector(".statwins")?.TextContent.Trim()),
                Losses = int.Parse(playerStatsPage.QuerySelector(".statlosses")?.TextContent.Trim()),
                Goals = int.Parse(playerStatsPage.QuerySelector(".statgoals")?.TextContent.Trim()),
                Assists = int.Parse(playerStatsPage.QuerySelector(".statgoal_assist")?.TextContent.Trim()),
                YellowCards = int.Parse(playerStatsPage.QuerySelector(".statyellow_card")?.TextContent.Trim()),
                RedCards = int.Parse(playerStatsPage.QuerySelector(".statred_card")?.TextContent.Trim()),

                CleanSheets = cleanSheets != null ? int.Parse(cleanSheets) : (int?)null,
                GoalsConceded = goalsConc != null ? int.Parse(goalsConc) : (int?)null,
                Fouls = fouls != null ? int.Parse(fouls) : (int?)null,
                Shots = shots != null ? int.Parse(shots) : (int?)null,
                ShotsOnTarget = shotsOnTarget != null ? int.Parse(shotsOnTarget) : (int?)null,
                Passes = passes != null ? int.Parse(passes) : (int?)null,
                BigChanceCreated = bigChanceCreated != null ? int.Parse(bigChanceCreated) : (int?)null,
                Tackles = tackles != null ? int.Parse(tackles) : (int?)null,
                Clearences = cleareances != null ? int.Parse(cleareances) : (int?)null,
            };

            return playerStatsTotalDto;
        }

        private Player CreatePlayerFromPlayerDto(PlayerDto playerDto, int clubId)
        {
            var player = new Player
            {
                ClubId = clubId,
                DateOfBirth = playerDto.DateOfBirth,
                FirstName = playerDto.FirstName,
                LastName = playerDto.LastName,
                ImageUrl = playerDto.ImageUrl,
                PLOverviewLink = playerDto.PLOverviewLink,
                PlayerStats = new PlayerStats
                {
                    Appearances = playerDto.PremierLeagueRecordTotal.Appearances,
                    CleanSheets = playerDto.PremierLeagueRecordTotal.CleanSheets,
                    GoalsConceded = playerDto.PremierLeagueRecordTotal.GoalsConceded,
                    Assists = playerDto.PremierLeagueRecordTotal.Assists,
                    Goals = playerDto.PremierLeagueRecordTotal.Goals,
                    Losses = playerDto.PremierLeagueRecordTotal.Losses,
                    RedCards = playerDto.PremierLeagueRecordTotal.RedCards,
                    Wins = playerDto.PremierLeagueRecordTotal.Wins,
                    YellowCards = playerDto.PremierLeagueRecordTotal.YellowCards,
                    BigChanceCreated = playerDto.PremierLeagueRecordTotal.BigChanceCreated,
                    Clearences = playerDto.PremierLeagueRecordTotal.Clearences,
                    Fouls = playerDto.PremierLeagueRecordTotal.Fouls,
                    Passes = playerDto.PremierLeagueRecordTotal.Passes,
                    Shots = playerDto.PremierLeagueRecordTotal.Shots,
                    ShotsOnTarget = playerDto.PremierLeagueRecordTotal.ShotsOnTarget,
                    Tackles = playerDto.PremierLeagueRecordTotal.Tackles,
                },
                Height = playerDto.Height,
                Weight = playerDto.Weight,
                SocialLinks = new SocialLinks
                {
                    FacebookLink = playerDto.SocialLinks.FacebookLink,
                    TweeterLink = playerDto.SocialLinks.TweeterLink,
                    InstagramLink = playerDto.SocialLinks.InstagramLink,
                },

                SquadNumber = playerDto.SquadNumber,
            };

            return player;
        }
    }
}
