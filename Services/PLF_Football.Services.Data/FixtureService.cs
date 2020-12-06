namespace PLF_Football.Services.Data
{
    using System.Linq;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;

    public class FixtureService : IFixtureService
    {
        private readonly IRepository<Fixture> fixturesRepo;

        public FixtureService(IRepository<Fixture> fixturesRepo)
        {
            this.fixturesRepo = fixturesRepo;
        }

        public int GetNextMatchday()
        {
            int day = 0;
            for (int i = 1; i <= 38; i++)
            {
                if (this.fixturesRepo.All().Where(x => x.Matchday == i).All(x => x.Started == false))
                {
                    day = i;
                    break;
                }
            }

            return day;
        }
    }
}
