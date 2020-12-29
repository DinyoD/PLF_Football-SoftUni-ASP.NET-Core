namespace PLF_Football.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Fixtures;
    using Xunit;

    public class FixturesServiceTests
    {

        [Fact]
        public async Task UpdateFixtureAsync()
        {
            var mockRepo = new Mock<IDeletableEntityRepository<Fixture>>();
            mockRepo.Setup(x => x.All()).Returns(this.fixture.AsQueryable());

            var service = new FixtureService(mockRepo.Object);
            var fixturesForUpdate = new List<FixtureForUpdateDto>
            {
                new FixtureForUpdateDto{ Id = 1, Result = "2-2"},
                new FixtureForUpdateDto{ Id = 3, Result = "2-1"},
            };
            await service.UpdateFixtureAsync(fixturesForUpdate);

            Assert.Equal("2-2", fixture.Where(x => x.Id == 1).FirstOrDefault().Result);
            Assert.Equal("2-1", fixture.Where(x => x.Id == 3).FirstOrDefault().Result);
        }

        private List<Fixture> fixture = new List<Fixture>
        {
            new Fixture { Id = 3, Result = "0-1", },
            new Fixture { Id = 2, Result = "0-1", },
            new Fixture { Id = 1, Result = "0-1", },
        };
    }
}
