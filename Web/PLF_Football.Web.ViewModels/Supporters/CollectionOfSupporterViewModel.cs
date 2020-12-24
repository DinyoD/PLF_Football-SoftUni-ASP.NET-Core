namespace PLF_Football.Web.ViewModels.Supporters
{
    using System.Collections.Generic;

    using PLF_Football.Web.ViewModels.UserGame;

    public class CollectionOfSupporterViewModel
    {
        public ICollection<SupporterViewModel> AllSupporters { get; set; }

        public IEnumerable<GamePointsViewModel> TopFiveGame { get; set; }
    }
}
