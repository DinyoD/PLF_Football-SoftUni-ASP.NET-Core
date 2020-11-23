namespace PLF_Football.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;
    using PLF_Football.Common;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Model;

    public class PlayersByClubScraperService : IPlayersByClubScraperService
    {
        private readonly IBrowsingContext context;

        public PlayersByClubScraperService()
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
        }

        public async Task ImportPlayersInfoAsync(string clubSquadLink)
        {
            var clubSquadPage = await this.context.OpenAsync(clubSquadLink);

            var players = clubSquadPage.QuerySelectorAll(".squadList > li > a");

            var playerDto = new ConcurrentBag<PlayerDto>();

            foreach (var item in players)
            {
                playerDto = await this.GetPlayersInfoAsync(item);

                var player = this.CreatePlayerFromPlayerDto(playerDto);
            }

        }

        public async Task<PlayerSeasonStatsDto> GetPlayerSeasonStats(PlayerDto player)
        {
            var playerSeasonStatsPage = await this.context.OpenAsync(player.PLCurrSeasonLink);
            var cleanSheets = playerSeasonStatsPage.QuerySelector(".statclean_sheet")?.TextContent.Trim();
            var goalsConc = playerSeasonStatsPage.QuerySelector(".statgoals_conceded")?.TextContent.Trim();

            var playerSeasonStatsDto = new PlayerSeasonStatsDto
            {
                AppearancesSeason = int.Parse(playerSeasonStatsPage.QuerySelector(".statappearances")?.TextContent.Trim()),
                WinsSeason = int.Parse(playerSeasonStatsPage.QuerySelector(".statwins")?.TextContent.Trim()),
                LossesSeason = int.Parse(playerSeasonStatsPage.QuerySelector(".statlosses")?.TextContent.Trim()),
                GoalsSeason = int.Parse(playerSeasonStatsPage.QuerySelector(".statgoals")?.TextContent.Trim()),
                AssistsSeason = int.Parse(playerSeasonStatsPage.QuerySelector(".statgoal_assist")?.TextContent.Trim()),
                YellowCardsSeason = int.Parse(playerSeasonStatsPage.QuerySelector(".statyellow_card")?.TextContent.Trim()),
                RedCardsSeason = int.Parse(playerSeasonStatsPage.QuerySelector(".statred_card")?.TextContent.Trim()),

                CleanSheetsSeason = cleanSheets != null ? int.Parse(cleanSheets) : -1,
                GoalsConcededSeason = goalsConc != null ? int.Parse(goalsConc) : -1,
            };
            return playerSeasonStatsDto;
        }

        private async Task<ConcurrentBag<PlayerDto>> GetPlayersInfoAsync(AngleSharp.Dom.IElement item)
        {
            var players = new ConcurrentBag<PlayerDto>();

            var player = new PlayerDto();

            player.PLOverviewLink = GlobalConstants.PremierLeagueLink
                .Replace("/clubs", string.Empty) + item.GetAttribute("href");

            player.PositionName = item.QuerySelector(".position")?.TextContent;

            var playerData = item.QuerySelector(".squadPlayerHeader > img").GetAttribute("data-player");
            player.ImageUrl = GlobalConstants.PlayerImgLink + playerData + ".png";

            var playerPage = await this.context.OpenAsync(player.PLOverviewLink);

            player.SquadNumber = playerPage.QuerySelector(".playerDetails > .number")?.TextContent;

            var fullName = playerPage.QuerySelector(".playerDetails  .name")?.TextContent;

            var names = fullName.Split(" ");
            if (names.Length > 1)
            {
                player.FirstName = names[0];
                player.LastName = names[1];
            }
            else
            {
                player.LastName = fullName;
            }

            player.NationalityName = playerPage.QuerySelector(".playerCountry")?.TextContent;

            var playerDateOfBirth =
                playerPage.QuerySelector(".pdcol2  div.info")?.TextContent.Trim().Split(" ").FirstOrDefault();

            player.DateOfBirth = DateTime.ParseExact(playerDateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var playerBodyStats = playerPage.QuerySelectorAll(".pdcol3  .info");

            if (playerBodyStats.Length > 1)
            {
                player.Height = int.Parse(playerBodyStats[0]?.TextContent.Replace("cm", string.Empty));
                player.Weight = int.Parse(playerBodyStats[1]?.TextContent.Replace("kg", string.Empty));
            }
            else if (playerBodyStats.Length == 1)
            {
                player.Height = int.Parse(playerBodyStats[0]?.TextContent.Replace("cm", string.Empty));
            }

            player.PremierLeagueRecordTotal = await this.GetPlayerStatsTotal(player);

            player.PremierLeagueSeasonRecord = await this.GetPlayerSeasonStats(player);

            player.SocialLinks = await this.GetPlayerSocialLinks(player);

            players.Add(player);

            return players;
        }

        private async Task<SocialLinksDto> GetPlayerSocialLinks(PlayerDto player)
        {
            var playerStatsPage = await this.context.OpenAsync(player.PLTotalStatsLink);

            var socilaLinksDto = new SocialLinksDto
            {
                FacebookLink = playerStatsPage.QuerySelector(".facebook-option")?.GetAttribute("href"),
                TweeterLink = playerStatsPage.QuerySelector(".twitter-option")?.GetAttribute("href"),
                InstagramLink = playerStatsPage.QuerySelector(".instagram-option")?.GetAttribute("href"),
            };

            return socilaLinksDto;
        }

        private async Task<PlayerStatsTotalDto> GetPlayerStatsTotal(PlayerDto player)
        {
            var playerStatsPage = await this.context.OpenAsync(player.PLTotalStatsLink);
            var cleanSheets = playerStatsPage.QuerySelector(".statclean_sheet")?.TextContent.Trim();
            var goalsConc = playerStatsPage.QuerySelector(".statgoals_conceded")?.TextContent.Trim();

            var playerStatsTotalDto = new PlayerStatsTotalDto
            {
                AppearancesTotal = int.Parse(playerStatsPage.QuerySelector(".statappearances")?.TextContent.Trim()),
                WinsTotal = int.Parse(playerStatsPage.QuerySelector(".statwins")?.TextContent.Trim()),
                LossesTotal = int.Parse(playerStatsPage.QuerySelector(".statlosses")?.TextContent.Trim()),
                GoalsTotal = int.Parse(playerStatsPage.QuerySelector(".statgoals")?.TextContent.Trim()),
                AssistsTotal = int.Parse(playerStatsPage.QuerySelector(".statgoal_assist")?.TextContent.Trim()),
                YellowCardsTotal = int.Parse(playerStatsPage.QuerySelector(".statyellow_card")?.TextContent.Trim()),
                RedCardsTotal = int.Parse(playerStatsPage.QuerySelector(".statred_card")?.TextContent.Trim()),

                CleanSheetsTotal = cleanSheets != null ? int.Parse(cleanSheets) : -1,
                GoalsConcededTotal = goalsConc != null ? int.Parse(goalsConc) : -1,
            };
            return playerStatsTotalDto;
        }

        private Player CreatePlayerFromPlayerDto(ConcurrentBag<PlayerDto> playerDto)
        {
            var player = new Player();

            return player;
        }
    }
}
