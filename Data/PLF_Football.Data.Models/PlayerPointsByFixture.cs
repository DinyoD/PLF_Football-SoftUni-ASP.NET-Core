namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;

    public class PlayerPointsByFixture : BaseDeletableModel<int>
    {
        public int PlayerId { get; set; }

        public Player Player { get; set; }

        public int FixtureId { get; set; }

        public Fixture Fixture { get; set; }

        public int Points { get; set; }
    }
}
