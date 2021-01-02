namespace PLF_Football.Web.ViewModels.ViewComponents
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class FixtureViewModel : IMapFrom<Fixture>
    {
        public ClubsLinksViewModel HomeTeam { get; set; }

        public ClubsLinksViewModel AwayTeam { get; set; }
    }
}
