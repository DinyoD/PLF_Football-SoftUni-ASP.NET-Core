namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Web.ViewModels.Fixtures;

    public interface IFixtureService
    {
        ICollection<T> GetFixtures<T>(int currMatchday);

        Task UpdateFixtureAsync(ICollection<FixtureForUpdateDto> fixtures);
    }
}
