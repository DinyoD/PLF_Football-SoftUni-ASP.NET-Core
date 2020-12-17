namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;

    public class PlayerPointsByFixture : BaseDeletableModel<int>
    {
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }
    }
}
