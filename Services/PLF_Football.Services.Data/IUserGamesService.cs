namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Data.Models;

    public interface IUserGamesService
    {
        Task AddPlayerToUserGameAsync(int playerId, int userGameId);

        UserGame GetUserGameByUserAndMatchday(string userId, int matchday);

        Task<UserGame> CreateUserGameAsync(string userId, int matchday);

        T GetUserGame<T>(string userId, int matchday);
    }
}
