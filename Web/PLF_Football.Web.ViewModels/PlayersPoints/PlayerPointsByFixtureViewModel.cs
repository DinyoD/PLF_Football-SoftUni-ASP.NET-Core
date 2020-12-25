namespace PLF_Football.Web.ViewModels.PlayersPoints
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayerPointsByFixtureViewModel : PointsByFixtureBasicViewModel, IMapFrom<PlayerPointsByFixture>
    {
        public int PlayerId { get; set; }
    }
}
