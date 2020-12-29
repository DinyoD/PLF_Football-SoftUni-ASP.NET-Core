namespace PLF_Football.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using Xunit;

    public class MessagesServiceTests
    {
        [Fact]
        public async Task AddMessageAsync()
        {
            var mockRepo = new Mock<IDeletableEntityRepository<Message>>();
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Message>())).Callback(
                (Message mess) => this.messages.Add(mess));

            var service = new MessagesService(mockRepo.Object);

            await service.AddMessageAssyns("d34", "ABC");

            Assert.Equal(4, this.messages.Count);
        }

        private List<Message> messages = new List<Message>
        {
            new Message { UserId = "a23", Text = "AAA", },
            new Message { UserId = "a24", Text = "BBB", },
            new Message { UserId = "b23", Text = "CCC", },
        };
    }
}
