namespace PLF_Football.Data.Models
{
    using System.Collections.Generic;

    using PLF_Football.Data.Common.Models;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.Players = new HashSet<Player>();
        }

        public string Name { get; set; }

        public string Flag { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
