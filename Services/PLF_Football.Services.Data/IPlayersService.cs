namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Data.Models;

    public interface IPlayersService
    {
        T GetPlayerStatsbyId<T>(int playerId);

        T GetPlayerById<T>(int? playerId);

        Player GetPlayerById(int playerId);

        ICollection<T> GetPlayersBySearchingString<T>(string searchString);

        ICollection<T> GetAll<T>();

        Task UpdatePlayerPrice(int id, int price);

        int GetCount();

        ICollection<T> GetPlayersByClubId<T>(int clubId);

        ICollection<Player> GetPlayersByClubId(int clubId);

        string GetClubNameByPLayerId(int playerId);
    }
}
