namespace PLF_Football.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using PLF_Football.Data.Common.Models;

    public class UserGame : BaseDeletableModel<int>
    {
        public UserGame()
        {
            this.MatchdayTeam = new HashSet<Player>();
        }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Player> MatchdayTeam { get; set; }
    }
}
