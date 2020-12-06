namespace PLF_Football.Web.ViewModels.Players
{
    using System;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayerStatsViewModel : IMapFrom<PlayerStats>
    {
        public int Appearances { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public string WinRatio => this.Appearances > 0 ? (int)(this.Wins * 100 / this.Appearances) + "%" : "0%";

        public int? CleanSheets { get; set; }

        public string CleanSheetsRatio => this.CleanSheets != null && this.Appearances > 0
            ? (int)(this.CleanSheets * 100 / this.Appearances) + "%"
            : "0%";

        public int? GoalsConceded { get; set; }

        public string GoalsConcededPerMatch => this.GoalsConceded != null && this.Appearances > 0
            ? ((double)this.GoalsConceded / (double)this.Appearances).ToString("0.##")
            : "0";

        public int Goals { get; set; }

        public string GoalsPerMatch => this.Appearances > 0
            ? ((double)this.Goals / (double)this.Appearances).ToString("0.##")
            : "0";

        public int Assists { get; set; }

        public string AssistsPerMatch => this.Appearances > 0
            ? ((double)this.Assists / (double)this.Appearances).ToString("0.##")
            : "0";

        public int YellowCards { get; set; }

        public string YellowCardsPerMatch => this.Appearances > 0
            ? ((double)this.YellowCards / (double)this.Appearances).ToString("0.#")
            : "0";

        public int RedCards { get; set; }

        public int? Fouls { get; set; }

        public string FoulsPerMatch => this.Fouls != null && this.Appearances > 0
            ? ((double)this.Fouls / (double)this.Appearances).ToString("0.##")
            : "0";

        public int? Shots { get; set; }

        public string ShotsPerMatch => this.Shots != null && this.Appearances > 0
           ? ((double)this.Shots / (double)this.Appearances).ToString("0.#")
           : "0";

        public int? ShotsOnTarget { get; set; }

        public string ShotsOnTargetPerMatch => this.ShotsOnTarget != null && this.Appearances > 0
           ? ((double)this.ShotsOnTarget / (double)this.Appearances).ToString("0.#")
           : "0";

        public int? Passes { get; set; }

        public string PassesPerMatch => this.Passes != null && this.Appearances > 0
           ? (this.Passes / this.Appearances).ToString()
           : "0";

        public int? BigChanceCreated { get; set; }

        public string BigChanceCreatedPerMatch => this.BigChanceCreated != null && this.Appearances > 0
           ? ((double)this.BigChanceCreated / (double)this.Appearances).ToString("0.#")
           : "0";

        public int? Clearences { get; set; }

        public string ClearencesPerMatch => this.Clearences != null && this.Appearances > 0
           ? (this.Clearences / this.Appearances).ToString()
           : "0";

        public int? Tackles { get; set; }

        public string TacklesPerMatch => this.Tackles != null && this.Appearances > 0
           ? (this.Tackles / this.Appearances).ToString()
           : "0";

        public int PlayerId { get; set; }

        public PlayerInClubMainVewModel Player { get; set; }
    }
}
