namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Data.Models;

    public interface IUserGamesService
    {
        int GetUserLastMatchdayInUserGames(string userId);

        Task<bool> AddPlayerToUserGameAsync(int playerId);

        ICollection<Player> GetPlayersInLastTeam(string userId);

        Task CreateUserGameAsync(string userId, int nextMatchday);
    }
}
