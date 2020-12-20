namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class ClubsService : IClubsService
    {
        private readonly IDeletableEntityRepository<Club> clubsRepo;

        public ClubsService(IDeletableEntityRepository<Club> clubsRepo)
        {
            this.clubsRepo = clubsRepo;
        }

        public ICollection<T> GetAll<T>()
        {
            return this.clubsRepo.AllAsNoTracking().OrderBy(x => x.Name).To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            return this.clubsRepo.AllAsNoTracking().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }
    }
}
