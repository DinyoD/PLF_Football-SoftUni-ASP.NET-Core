namespace PLF_Football.Data.Models
{
    using System.Collections.Generic;

    using PLF_Football.Data.Common.Models;

    public class Position : BaseModel<int>
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        public string Name { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
