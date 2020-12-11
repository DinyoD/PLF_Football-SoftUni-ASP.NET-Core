namespace PLF_Football.Web.Areas.Administration.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayerAdminService : IPlayerAdminService
    {
        private readonly IDeletableEntityRepository<Player> playersRepo;

        public PlayerAdminService(IDeletableEntityRepository<Player> playersRepo)
        {
            this.playersRepo = playersRepo;
        }

        public ICollection<T> GetAll<T>()
        {
            return this.playersRepo.All().OrderBy(x => x.LastName).To<T>().ToList();
        }

        public T GetPlayerbyId<T>(int? id)
        {
            return this.playersRepo.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
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
