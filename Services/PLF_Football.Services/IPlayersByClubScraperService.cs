namespace PLF_Football.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Services.Model;

    public interface IPlayersByClubScraperService
    {
        Task ImportPlayersAsync();

        Task<ICollection<UpdatedPlayerDto>> GetPlayersInClubNewStatsAsync(int clubId);
    }
}
