namespace PLF_Football.Web.ViewModels.ViewComponents
{
    using System.Collections.Generic;

    public class NextFixturesListViewModel
    {
        public int Matchday { get; set; }

        public IEnumerable<FixtureViewModel> Fixture { get; set; }
    }
}
