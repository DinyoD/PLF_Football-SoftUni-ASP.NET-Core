namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlayersService
    {
        T GetPlayerStatsbyId<T>(int playerId);

        T GetPlayerById<T>(int? playerId);

        ICollection<T> GetPlayersBySearchingString<T>(string searchString);

        ICollection<T> GetAll<T>();

        Task UpdatePlayerPrice(int id, int price);

        int GetCount();
    }
}
