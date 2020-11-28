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

        public string FlagCode { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
