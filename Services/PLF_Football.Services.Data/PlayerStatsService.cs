namespace PLF_Football.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Web.ViewModels.Players;

    public class PlayerStatsService : IPlayerStatsService
    {
        private readonly IDeletableEntityRepository<PlayerStats> statsRepo;

        public PlayerStatsService(IDeletableEntityRepository<PlayerStats> statsRepo)
        {
            this.statsRepo = statsRepo;
        }

        public async Task UpdateStatsAsync(PlayerStatsForUpdateDto newPlayersStats)
        {
            var playerStats = this.statsRepo.All().Where(x => x.PlayerId == newPlayersStats.PlayerId).FirstOrDefault();
            if (playerStats != null)
            {
                playerStats.Appearances = newPlayersStats.Appearances;
                playerStats.Wins = newPlayersStats.Wins;
                playerStats.Losses = newPlayersStats.Losses;
                playerStats.CleanSheets = newPlayersStats.CleanSheets;
                playerStats.Clearences = newPlayersStats.Clearences;
                playerStats.Tackles = newPlayersStats.Tackles;
                playerStats.GoalsConceded = newPlayersStats.GoalsConceded;
                playerStats.Goals = newPlayersStats.Goals;
                playerStats.Shots = newPlayersStats.Shots;
                playerStats.ShotsOnTarget = newPlayersStats.ShotsOnTarget;
                playerStats.Assists = newPlayersStats.Assists;
                playerStats.Fouls = newPlayersStats.Fouls;
                playerStats.BigChanceCreated = newPlayersStats.BigChanceCreated;
                playerStats.Passes = newPlayersStats.Passes;
                playerStats.YellowCards = newPlayersStats.YellowCards;
                playerStats.RedCards = newPlayersStats.RedCards;

                await this.statsRepo.SaveChangesAsync();
            }
        }
    }
}
