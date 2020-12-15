﻿namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PLF_Football.Common;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Data.Models;
    using PLF_Football.Services.Mapping;

    public class FixtureService : IFixtureService
    {
        private readonly IRepository<Fixture> fixturesRepo;

        public FixtureService(IRepository<Fixture> fixturesRepo)
        {
            this.fixturesRepo = fixturesRepo;
        }

        public ICollection<T> GetFixtures<T>(int nextNotStartedMatchday)
        {
            return this.fixturesRepo
                      .All()
                      .Where(x => x.Matchday < nextNotStartedMatchday)
                      .To<T>()
                      .ToList();
        }

        public int GetNextMatchday()
        {
            int day = 0;
            for (int i = 1; i <= GlobalConstants.AllFixtureCount; i++)
            {
                if (this.fixturesRepo.All().Where(x => x.Matchday == i).All(x => x.Started == false))
                {
                    day = i;
                    break;
                }
            }

            return day;
        }

        public Task UpdateNewFinishedFixture(ICollection<FixtureForUpdateDto> fixturesForUpdate)
        {
            return null;
        }
    }
}