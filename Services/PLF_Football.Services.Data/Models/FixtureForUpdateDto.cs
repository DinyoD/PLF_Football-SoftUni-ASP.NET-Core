namespace PLF_Football.Services.Data.Models
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class FixtureForUpdateDto : IMapFrom<Fixture>
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public string Result { get; set; }

        public bool Started { get; set; }

        public bool Finished { get; set; }
    }
}
