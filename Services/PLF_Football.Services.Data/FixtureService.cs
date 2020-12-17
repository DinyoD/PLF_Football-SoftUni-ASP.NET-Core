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

        public ICollection<T> GetFixtures<T>(int currMatchday)
        {
            return this.fixturesRepo
                        .All()
                        .Where(x => x.Matchday <= currMatchday)
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
