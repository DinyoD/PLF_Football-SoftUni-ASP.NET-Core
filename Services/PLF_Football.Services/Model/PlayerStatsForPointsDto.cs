namespace PLF_Football.Services.Model
{

    public class PlayerStatsForPointsDto
    {
        public int Appearances { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int? CleanSheets { get; set; }

        public int? GoalsConceded { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }

        public int YellowCards { get; set; }

        public int RedCards { get; set; }
    }
}
