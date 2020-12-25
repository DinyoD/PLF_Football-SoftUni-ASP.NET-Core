namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Web.ViewModels.Fixtures;

    public interface IFixtureService
    {
        ICollection<T> GetFixturesOnAndBeforeSpecificMatchday<T>(int matchday);

        ICollection<T> GetFixturesOnAndBeforeSpecificMatchdayByClub<T>(int matchday, int clubId);

        ICollection<T> GetFixturesAfterSpecificAndBeforeOrOnNextMatchday<T>(int matchday, int nextmatchday);

        ICollection<T> GetAllFixtures<T>();
            
        Task UpdateFixtureAsync(ICollection<FixtureForUpdateDto> fixtures);
    }
}
