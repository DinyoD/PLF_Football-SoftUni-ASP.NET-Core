namespace PLF_Football.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Services.Model;

    public interface IFixtureScraperService
    {
        Task ImportFixture();

        Task<ICollection<FixtureDto>> GetFixture(int nextNotStartedMatchday);
    }
}
