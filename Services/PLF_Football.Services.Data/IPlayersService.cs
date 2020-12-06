
using System.Threading.Tasks;

namespace PLF_Football.Services.Data
{
    public interface IPlayersService
    {
        T GetPlayerStatsbyId<T>(int playerId);
    }
}
