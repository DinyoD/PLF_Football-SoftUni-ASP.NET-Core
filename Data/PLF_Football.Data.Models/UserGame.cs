namespace PLF_Football.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using PLF_Football.Data.Common.Models;

    public class UserGame : BaseDeletableModel<int>
    {
        public UserGame()
        {
            this.MatchdayTeam = new HashSet<PlayerUserGame>();
        }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int Matchday { get; set; }

        public int Points => this.MatchdayTeam
                                        .Sum(x => x.Player.PlayerPoints
                                                    .Where(y => y.Matchday == this.Matchday)
                                                    .Select(z => z.Points)
                                                    .FirstOrDefault());

        public virtual ICollection<PlayerUserGame> MatchdayTeam { get; set; }
    }
}
