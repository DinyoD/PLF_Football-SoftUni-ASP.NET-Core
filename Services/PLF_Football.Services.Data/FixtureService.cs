namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Fixtures;

    public class FixtureService : IFixtureService
    {
        private readonly IRepository<Fixture> fixturesRepo;

        public FixtureService(IDeletableEntityRepository<Fixture> fixturesRepo)
        {
            this.fixturesRepo = fixturesRepo;
        }

        public ICollection<T> GetFixturesAfterSpecificAndBeforeOrOnNextMatchday<T>(int matchday, int nextMatchday)
        {
            if (nextMatchday > 38)
            {
                nextMatchday = 38;
            }

            return this.fixturesRepo
                        .All()
                        .Where(x => x.Matchday > matchday && x.Matchday <= nextMatchday)
                        .To<T>()
                        .ToList();
        }

        public ICollection<T> GetFixturesOnAndBeforeSpecificMatchday<T>(int matchday)
        {
            return this.fixturesRepo
                        .All()
                        .Where(x => x.Matchday <= matchday)
                        .To<T>()
                        .ToList();
        }

        public ICollection<T> GetFixturesOnAndBeforeSpecificMatchdayByClub<T>(int matchday, int clubId)
        {
            return this.fixturesRepo
                        .All()
                        .Where(x => x.Matchday <= matchday && (x.HomeTeamId == clubId || x.AwayTeamId == clubId))
                        .To<T>()
                        .ToList();
        }

        public async Task UpdateFixtureAsync(ICollection<FixtureForUpdateDto> fixtures)
        {
            foreach (var fixture in fixtures)
            {
                var currFixture = this.fixturesRepo.All().Where(x => x.Id == fixture.Id).FirstOrDefault();

                currFixture.Started = fixture.Started;
                currFixture.Finished = fixture.Finished;
                currFixture.Result = fixture.Result;
            }

            await this.fixturesRepo.SaveChangesAsync();
        }
    }
}
