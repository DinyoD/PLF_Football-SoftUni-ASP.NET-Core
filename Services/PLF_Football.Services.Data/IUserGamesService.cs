namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Data.Models;

    public interface IUserGamesService
    {
        Task AddPlayerToUserGameAsync(int playerId, int userGameId);

        int GetUserGameIdByUserAndMatchday(string userId, int matchday);

        Task<int> CreateUserGameAsync(string userId, int matchday);

        ICollection<Player> GetUserGameMatchTeamPlayers(int userGameId);

        T GetUserGame<T>(string userId, int matchday);
    }
}
