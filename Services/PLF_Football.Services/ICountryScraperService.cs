namespace PLF_Football.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Data.Models;

    public interface ICountryScraperService
    {
        Task ImportPlayersCountry();
    }
}
