namespace PLF_Football.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Moq;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Clubs;
    using Xunit;

    public class ClubsServiceTests
    {
        [Fact]
        public void GetAllClubs()
        {
            var mockRepo = new Mock<IDeletableEntityRepository<Club>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(this.clubs.AsQueryable());

            var service = new ClubsService(mockRepo.Object);
            var allClubs = service.GetAll<ClubDto>();

            Assert.Equal(3, allClubs.Count);
        }

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

        private class ClubDto : IMapFrom<Club>
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
