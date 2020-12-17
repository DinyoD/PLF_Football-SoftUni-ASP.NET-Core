namespace PLF_Football.Services.Model
{
    public class FixtureDto
    {
        public int Matchday { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public string Result { get; set; }

        public bool Started { get; set; }

        public bool Finished { get; set; }
    }
}
