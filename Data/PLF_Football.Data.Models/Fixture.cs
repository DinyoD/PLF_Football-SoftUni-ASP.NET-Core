namespace PLF_Football.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using PLF_Football.Data.Common.Models;

    public class Fixture : BaseDeletableModel<int>
    {
        public int Matchday { get; set; }

        public int HomeTeamId { get; set; }

        [ForeignKey("HomeTeamId")]
        public Club HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        [ForeignKey("AwayTeamId")]
        public Club AwayTeam { get; set; }

        public string Result { get; set; }

        public bool Started { get; set; }

        public bool Finished { get; set; }
    }
}
