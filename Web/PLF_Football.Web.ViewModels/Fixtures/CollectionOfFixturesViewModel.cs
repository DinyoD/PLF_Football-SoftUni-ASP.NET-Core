namespace PLF_Football.Web.ViewModels.Fixtures
{
    using System.Collections.Generic;

    public class CollectionOfFixturesViewModel
    {
        public int UserTeamId { get; set; }

        public ICollection<FixtureBasicViewModel> AllFixtures { get; set; }
    }
}
