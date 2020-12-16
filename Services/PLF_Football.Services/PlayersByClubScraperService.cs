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
        private readonly IRepository<Position> positionRepo;
        private readonly IRepository<Country> countryRepo;

        public PlayersByClubScraperService(
            IRepository<Club> clubRepo,
            IRepository<Position> positionRepo,
            IRepository<Country> countryRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.clubRepo = clubRepo;
            this.positionRepo = positionRepo;
            this.countryRepo = countryRepo;
        }

        public async Task ImportPlayersAsync()
        {
            foreach (var club in this.clubRepo.All())
            {
                var players = await this.GetClubPlayersAsync(club);

                club.Players = players;
            }
        }

        public async Task<ICollection<PlayerInfoForUpdateDto>> GetPlayersNewStatsAsync(ICollection<int> clubsId)
        {
            var players = new List<PlayerInfoForUpdateDto>();
            foreach (var clubId in clubsId)
            {
                var club = this.clubRepo.AllAsNoTracking().Where(x => x.Id == clubId).FirstOrDefault();

                var clubSquadPage = await this.context.OpenAsync(club.PLSquadLink);

                var playersInClub = clubSquadPage.QuerySelectorAll(".squadList > li > a");

                foreach (var player in playersInClub)
                {
                    var playerInfoForUpdateDto = new PlayerInfoForUpdateDto
                    {
                        PLOverviewLink = GlobalConstants.PremierLeagueLink
                                                            .Replace("/clubs", string.Empty) + player.GetAttribute("href"),
                        ClubId = clubId,
                        SquadNumber = player.QuerySelector(".number")?.TextContent,
                    };
                    playerInfoForUpdateDto.PremierLeagueRecordTotal = await this.GetPlayerStatsAsync(playerInfoForUpdateDto.PLTotalStatsLink);

                    players.Add(playerInfoForUpdateDto);
                }
            }

            return players;
        }

        private async Task<ICollection<Player>> GetClubPlayersAsync(Club club)
        {
            var playersDtoByClub = await this.GetPlayersDtoByClub(club);
            var players = new List<Player>();

            foreach (var playerDto in playersDtoByClub)
            {
                var player = this.CreatePlayerFromPlayerDto(playerDto, club.Id);

                player.Position = this.positionRepo
                                    .All()
                                    .FirstOrDefault(x => x.Name == playerDto.PositionName);
                player.Country = this.countryRepo
                                    .All()
                                    .FirstOrDefault(x => x.Name == playerDto.CountryName);

                player.Price = this.GetPlayerPrice(player.PlayerStats);

                players.Add(player);
            }

            return players;
        }

        private int GetPlayerPrice(PlayerStats playerStats)
        {
            var price = 200_000;

            if (playerStats.Appearances > 6)
            {
                var appearancesBonus = (playerStats.Wins * 100 / playerStats.Appearances) * 40_000;
                price += appearancesBonus;
            }

            if (playerStats.CleanSheets == null && playerStats.Appearances > 0)
            {
                var attackingBonus =
                    (playerStats.Goals * 100 / playerStats.Appearances * 60_000)
                    + (playerStats.Assists * 100 / playerStats.Appearances * 40_000);

                price += attackingBonus;
            }

            if (playerStats.CleanSheets != null && playerStats.Appearances > 0)
            {
                var goalkeepersBonus =
                    (playerStats.CleanSheets * 100 / playerStats.Appearances * 30_000)
                    - (playerStats.GoalsConceded / playerStats.Appearances * 10_000);

                price += (int)goalkeepersBonus;
            }

            if (playerStats.Clearences != null && playerStats.Appearances > 5)
            {
                var defendersgBonus =
                    (playerStats.Clearences * 10 / playerStats.Appearances * 10_000)
                    + (playerStats.Tackles * 10 / playerStats.Appearances * 10_000);

                price += (int)defendersgBonus;
            }

            return price;
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
            playerDto.SquadNumber = item.QuerySelector(".number")?.TextContent;

            var playerData = item.QuerySelector(".squadPlayerHeader > img").GetAttribute("data-player");
            playerDto.ImageUrl = GlobalConstants.PlayerImgLink + playerData + ".png";

            var playerPage = await this.context.OpenAsync(playerDto.PLOverviewLink);

            var fullName = playerPage.QuerySelector(".playerDetails  .name")?.TextContent;

            var names = fullName?.Split(" ", 2);
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

            playerDto.PremierLeagueRecordTotal = await this.GetPlayerStatsAsync(playerDto.PLTotalStatsLink);
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

        private async Task<PlayerStatsDto> GetPlayerStatsAsync(string playerStatsLink)
        {
            var playerStatsPage = await this.context.OpenAsync(playerStatsLink);

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
            var appearances = playerStatsPage.QuerySelector(".statappearances")?.TextContent.Trim();
            var wins = playerStatsPage.QuerySelector(".statwins")?.TextContent.Trim();
            var losses = playerStatsPage.QuerySelector(".statlosses")?.TextContent.Trim();
            var goals = playerStatsPage.QuerySelector(".statgoals")?.TextContent.Trim();
            var assists = playerStatsPage.QuerySelector(".statgoal_assist")?.TextContent.Trim();
            var yellowCards = playerStatsPage.QuerySelector(".statyellow_card")?.TextContent.Trim();
            var redCards = playerStatsPage.QuerySelector(".statred_card")?.TextContent.Trim();

            var playerStatsTotalDto = new PlayerStatsDto
            {
                Appearances = appearances != null ? int.Parse(appearances) : 0,
                Wins = wins != null ? int.Parse(wins) : 0,
                Losses = losses != null ? int.Parse(losses) : 0,
                Goals = goals != null ? int.Parse(goals) : 0,
                Assists = assists != null ? int.Parse(assists) : 0,
                YellowCards = yellowCards != null ? int.Parse(yellowCards) : 0,
                RedCards = redCards != null ? int.Parse(redCards) : 0,

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
