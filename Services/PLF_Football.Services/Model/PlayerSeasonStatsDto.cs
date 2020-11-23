namespace PLF_Football.Services.Model
{
    public class PlayerSeasonStatsDto
    {
        public int AppearancesSeason { get; set; }

        public int WinsSeason { get; set; }

        public int LossesSeason { get; set; }

        public int? CleanSheetsSeason { get; set; }

        public int? GoalsConcededSeason { get; set; }

        public int GoalsSeason { get; set; }

        public int AssistsSeason { get; set; }

        public int YellowCardsSeason { get; set; }

        public int RedCardsSeason { get; set; }

        public int PlayerId { get; set; }
    }
}
