namespace PLF_Football.Web.ViewModels.Fixtures
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class FixtureBasicViewModel : IMapFrom<Fixture>
    {
        public int Id { get; set; }

        public int Matchday { get; set; }

        public string HomeTeamName { get; set; }

        public int HomeTeamId { get; set; }

        public string AwayTeamName { get; set; }

        public int AwayTeamId { get; set; }

        public string Result { get; set; }

        public bool Finished { get; set; }
    }
}
