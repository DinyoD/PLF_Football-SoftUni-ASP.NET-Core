namespace PLF_Football.Services
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using PLF_Football.Services.Model;

    public interface IPlayersByClubScraperService
    {
        Task ImportPlayersInfoAsync(string clubSquadLink);
    }
}
