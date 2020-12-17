namespace PLF_Football.Services.Data
{
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Web.ViewModels.Players;

    public class PlayersPointsService : IPlayersPointsService
    {
        private readonly IDeletableEntityRepository<PlayerPointsByFixture> playersPointsRepo;

        public PlayersPointsService(IDeletableEntityRepository<PlayerPointsByFixture> playersPointsRepo)
        {
            this.playersPointsRepo = playersPointsRepo;
        }

        public async Task AddPlayersPointByFixtureAsync(PlayersPointsInMatchdayDto playersPointsDto)
        {
            var playersPoints = new PlayerPointsByFixture
            {
                Matchday = playersPointsDto.Matchday,
                PlayerId = playersPointsDto.PlayerId,
                Points = playersPointsDto.Points,
            };

            await this.playersPointsRepo.AddAsync(playersPoints);
            await this.playersPointsRepo.SaveChangesAsync();
        }
    }
}
