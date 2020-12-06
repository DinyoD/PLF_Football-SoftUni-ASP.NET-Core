
namespace PLF_Football.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PLayersService : IPlayersService
    {
        private readonly IDeletableEntityRepository<Player> playersRepo;

        public PLayersService(IDeletableEntityRepository<Player> playersRepo)
        {
            this.playersRepo = playersRepo;
        }

        public T GetPlayerStatsbyId<T>(int playerId)
        {
            return this.playersRepo.AllAsNoTracking()
                .Where(x => x.Id == playerId)
                .Select(x => x.PlayerStats)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
