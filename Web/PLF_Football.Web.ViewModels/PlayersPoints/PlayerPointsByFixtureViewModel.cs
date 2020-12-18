namespace PLF_Football.Web.ViewModels.PlayersPoints
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayerPointsByFixtureViewModel : IMapFrom<PlayerPointsByFixture>
    {
        public int PlayerId { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }
    }
}
