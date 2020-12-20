namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PositionService : IPositionService
    {
        private readonly IRepository<Position> positionRepo;

        public PositionService(IRepository<Position> positionRepo)
        {
            this.positionRepo = positionRepo;
        }

        public ICollection<T> GetAll<T>()
        {
            return this.positionRepo.AllAsNoTracking().OrderByDescending(x => x.Id).To<T>().ToList();
        }
    }
}
