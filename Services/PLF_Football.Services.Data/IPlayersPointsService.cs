namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PLF_Football.Web.ViewModels.Players;

    public interface IPlayersPointsService
    {
        Task AddPlayersPointByFixtureAsync(PlayersPointsInMatchdayDto playersPointsDto);

        int GetPointsByMatchdayAndPlayerIdCollection(int matchday, ICollection<int> playersId);
    }
}
