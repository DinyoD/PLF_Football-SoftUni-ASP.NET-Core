namespace PLF_Football.Services.Data
{
    using System.Threading.Tasks;

    using PLF_Football.Web.ViewModels.Players;

    public interface IPlayersPointsService
    {
        Task AddPlayersPointByFixtureAsync(PlayersPointsInMatchdayDto playersPointsDto);
    }
}
