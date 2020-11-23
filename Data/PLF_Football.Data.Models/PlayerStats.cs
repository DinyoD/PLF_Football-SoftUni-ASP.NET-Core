namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;

    public class PlayerStats : BaseDeletableModel<int>
    {
        public int AppearancesTotal { get; set; }

        public int AppearancesSeason { get; set; }

        public int WinsTotal { get; set; }

        public int WinsSeason { get; set; }

        public int LossesTotal { get; set; }

        public int LossesSeason { get; set; }

        public int? CleanSheetsTotal { get; set; }

        public int? CleanSheetsSeason { get; set; }

        public int? GoalsConcededTotal { get; set; }

        public int? GoalsConcededSeason { get; set; }

        public int GoalsTotal { get; set; }

        public int GoalsSeason { get; set; }

        public int AssistsTotal { get; set; }

        public int AssistsSeason { get; set; }

        public int YellowCardsTotal { get; set; }

        public int YellowCardsSeason { get; set; }

        public int RedCardsTotal { get; set; }

        public int RedCardsSeason { get; set; }
    }
}
