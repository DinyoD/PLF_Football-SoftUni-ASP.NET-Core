namespace PLF_Football.Services
{
    using System.Threading.Tasks;

    using PLF_Football.Services.Model;

    public interface IPlayerUpdatedStatsScraperService
    {
        Task<PlayerStatsForPointsDto> GetPlayerUpdatedStats(int playerId);
    }
}
