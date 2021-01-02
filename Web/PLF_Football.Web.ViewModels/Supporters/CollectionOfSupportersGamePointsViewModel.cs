namespace PLF_Football.Web.ViewModels.Supporters
{
    using System.Collections.Generic;

    using PLF_Football.Web.ViewModels.UserGame;

    public class CollectionOfSupportersGamePointsViewModel : PagingViewModel
    {
        public ICollection<GamePointsViewModel> Supporters { get; set; }

        public ICollection<MatchdayViewModel> AllMatchdays { get; set; }

        public int MatchdaySearch { get; set; }
    }
}
