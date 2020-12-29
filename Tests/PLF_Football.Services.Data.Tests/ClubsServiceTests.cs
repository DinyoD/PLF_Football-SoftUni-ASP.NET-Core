namespace PLF_Football.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using Xunit;

    public class ClubsServiceTests
    {

        [Fact]
        public void GetClubNameByClubId()
        {
            var mockRepo = new Mock<IDeletableEntityRepository<Club>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(this.clubs.AsQueryable());

            var service = new ClubsService(mockRepo.Object);

            var clubName = service.GetClubNameById(3);

            Assert.Equal("AAA", clubName);
        }

        private List<Club> clubs = new List<Club>
        {
            new Club { Id = 3, Name = "AAA", },
            new Club { Id = 2, Name = "BBB", },
            new Club { Id = 1, Name = "CCC", },
        };
    }
}
