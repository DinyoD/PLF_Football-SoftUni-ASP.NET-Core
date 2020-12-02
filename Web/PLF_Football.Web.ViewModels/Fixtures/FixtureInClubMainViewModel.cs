namespace PLF_Football.Web.ViewModels.Fixtures
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class FixtureInClubMainViewModel : IMapFrom<Fixture>
    {
        public int Matchday { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public string Result { get; set; }

        public bool Finished { get; set; }
    }
}
