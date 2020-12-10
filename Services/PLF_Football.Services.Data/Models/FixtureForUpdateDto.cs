namespace PLF_Football.Services.Data.Models
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class FixtureForUpdateDto : IMapFrom<Fixture>
    {
        public int Matchday { get; set; }

        public Club HomeTeam { get; set; }

        public Club AwayTeam { get; set; }

        public string Result { get; set; }

        public bool Started { get; set; }

        public bool Finished { get; set; }
    }
}
