namespace PLF_Football.Data.Models
{
    using System.Collections.Generic;

    using PLF_Football.Data.Common.Models;

    public class UserGames : BaseDeletableModel<int>
    {
        public UserGames()
        {
            this.MatchdayTeam = new HashSet<Player>();
        }

        public ApplicationUser User { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Player> MatchdayTeam { get; set; }
    }
}
