namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PlayerStats : BaseDeletableModel<int>
    {
        public int Appearances { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int? CleanSheets{ get; set; }

        public int? GoalsConceded { get; set; }

        public int Goals{ get; set; }

        public int Assists { get; set; }

        public int YellowCards { get; set; }

        public int RedCards { get; set; }

        public int? Fouls { get; set; }

        public int? Shots { get; set; }

        public int? ShotsOnTarget { get; set; }

        public int? Passes { get; set; }

        public int? BigChanceCreated { get; set; }

        public int? Clearences { get; set; }

        public int? Tackles { get; set; }

        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
    }
}
