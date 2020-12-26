namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepo;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepo)
        {
            this.messagesRepo = messagesRepo;
        }

        public async Task AddMessageAssyns(string userId, string text)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(text))
            {
                await this.messagesRepo.AddAsync(new Message { UserId = userId, Text = text });
                await this.messagesRepo.SaveChangesAsync();
            }
        }

        public ICollection<T> GetLastHundred<T>()
        {
            return this.messagesRepo.AllAsNoTracking().To<T>().Take(100).ToList();
        }
    }
}
