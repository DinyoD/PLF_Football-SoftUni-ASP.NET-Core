namespace PLF_Football.Services.Data
{
    using System.Threading.Tasks;

    public interface IUserGamesService
    {
        Task<bool> AddPlayerToUserGame(int playerId, int userGameId);
    }
}
