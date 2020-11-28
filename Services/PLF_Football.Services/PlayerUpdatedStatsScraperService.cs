namespace PLF_Football.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Model;

    public class PlayerUpdatedStatsScraperService : IPlayerUpdatedStatsScraperService
    {
        private readonly IBrowsingContext context;
        private readonly IDeletableEntityRepository<Player> playersRepo;

        public PlayerUpdatedStatsScraperService(IDeletableEntityRepository<Player> playersRepo)
        {
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
            this.playersRepo = playersRepo;
        }

        public async Task<PlayerStatsForPointsDto> GetPlayerUpdatedStats(int playerId)
        {
            var player = this.playersRepo.AllAsNoTracking().FirstOrDefault(x => x.Id == playerId);

            var playerStatsLink = player.PLTotalStatsLink;
            var playerStatsPage = await this.context.OpenAsync(playerStatsLink);

            var cleanSheets = playerStatsPage.QuerySelector(".statclean_sheet")?.TextContent.Trim();
            var goalsConc = playerStatsPage.QuerySelector(".statgoals_conceded")?.TextContent.Trim();

            var playerStatsForPointsDto = new PlayerStatsForPointsDto
            {
                Appearances = int.Parse(playerStatsPage.QuerySelector(".statappearances")?.TextContent.Trim()),
                Wins = int.Parse(playerStatsPage.QuerySelector(".statwins")?.TextContent.Trim()),
                Losses = int.Parse(playerStatsPage.QuerySelector(".statlosses")?.TextContent.Trim()),
                Goals = int.Parse(playerStatsPage.QuerySelector(".statgoals")?.TextContent.Trim()),
                Assists = int.Parse(playerStatsPage.QuerySelector(".statgoal_assist")?.TextContent.Trim()),
                YellowCards = int.Parse(playerStatsPage.QuerySelector(".statyellow_card")?.TextContent.Trim()),
                RedCards = int.Parse(playerStatsPage.QuerySelector(".statred_card")?.TextContent.Trim()),

                CleanSheets = cleanSheets != null ? int.Parse(cleanSheets) : -1,
                GoalsConceded = goalsConc != null ? int.Parse(goalsConc) : -1,
            };
            return playerStatsForPointsDto;
        }
    }
}
