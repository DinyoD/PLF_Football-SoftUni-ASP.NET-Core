namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Web.ViewModels.Fixtures;

    public interface IFixtureService
    {
        ICollection<T> GetFixturesOnAndBeforeSpecificMatchday<T>(int matchday);

        ICollection<T> GetFixturesBetweenSpecificAndNextMatchday<T>(int matchday, int nextmatchday);

        Task UpdateFixtureAsync(ICollection<FixtureForUpdateDto> fixtures);
    }
}
