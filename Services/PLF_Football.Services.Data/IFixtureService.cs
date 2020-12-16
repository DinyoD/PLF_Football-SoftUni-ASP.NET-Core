namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Services.Data.Models;

    public interface IFixtureService
    {
        int GetNextMatchday();

        ICollection<T> GetFixtures<T>(int nextNotStartedMatchday);

        Task<ICollection<int>> UpdateFixtureAsync(ICollection<FixtureForUpdateDto> fixtures);
    }
}
