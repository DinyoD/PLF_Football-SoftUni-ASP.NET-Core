namespace PLF_Football.Services.Data
{
    using PLF_Football.Data.Models;

    public interface IPlayersService
    {
        T GetPlayerStatsbyId<T>(int playerId);

        Player GetPlayerById(int playerId);
    }
}
