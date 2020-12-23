namespace PLF_Football.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Services.Model;

    public interface IFixtureScraperService
    {
        Task ImportFixturesAsync();

        Task<ICollection<FixtureDto>> GetFixturesOnAndBeforeMatchdayAsync(int matchday);

        Task<ICollection<FixtureDto>> GetFixturesOnMatchdayAsync(int matchday);

        Task<int> GetFirstNotStartedMatchdayAsync();
    }
}
