namespace PLF_Football.Services
{
    using System.Threading.Tasks;

    using PLF_Football.Data.Models;

    public interface IPlayersByClubScraperService
    {
        Task ImportPlayersAsync();
    }
}
