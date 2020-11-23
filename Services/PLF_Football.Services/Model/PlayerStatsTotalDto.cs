namespace PLF_Football.Services.Model
{
    public class PlayerStatsTotalDto
    {
        public int AppearancesTotal { get; set; }

        public int WinsTotal { get; set; }

        public int LossesTotal { get; set; }

        public int? CleanSheetsTotal { get; set; }

        public int? GoalsConcededTotal { get; set; }

        public int GoalsTotal { get; set; }

        public int AssistsTotal { get; set; }

        public int YellowCardsTotal { get; set; }

        public int RedCardsTotal { get; set; }

        public int PlayerId { get; set; }
    }
}
