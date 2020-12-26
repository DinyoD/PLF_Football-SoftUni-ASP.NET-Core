namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        ICollection<T> GetLastHundred<T>();

        Task AddMessageAssyns(string userId, string text);
    }
}
