namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
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

        public ICollection<T> GetAll<T>()
        {
            return this.playersRepo
                .All()
                //.OrderBy(x => x.LastName)
                //.Skip((pageNumber - 1) * playersPerPage)
                //.Take(playersPerPage)
                .To<T>()
                .ToList();
        }

        public int GetCount()
        {
            return this.playersRepo.All().Count();
        }

        public T GetPlayerById<T>(int? playerId)
        {
            return this.playersRepo.AllAsNoTracking().Where(x => x.Id == playerId).To<T>().FirstOrDefault();
        }

        public T GetPlayerStatsbyId<T>(int playerId)
        {
            return this.playersRepo.AllAsNoTracking()
                .Where(x => x.Id == playerId)
                .Select(x => x.PlayerStats)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task UpdatePlayerPrice(int id, int price)
        {
            var player = this.playersRepo.All().Where(x => x.Id == id).FirstOrDefault();
            if (player != null)
            {
                player.Price = price;
                await this.playersRepo.SaveChangesAsync();
            }
        }
    }
}
