namespace PLF_Football.Services
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using PLF_Football.Services.Model;

    public interface IPLClubsScraperService
    {
        Task ImportClubsInfoAsync();
    }
}
