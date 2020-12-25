namespace PLF_Football.Web.ViewModels.Players
{
    using System.Collections.Generic;

    using PLF_Football.Web.ViewModels.PlayersPoints;

    public class PlayerPointsViewModel
    {
        public string ClubName { get; set; }

        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public ICollection<PointsByFixtureViewModel> PointsByFixture { get; set; }
    }
}
