namespace PLF_Football.Web.ViewModels.Supporters
{
    using System.Collections.Generic;

    using PLF_Football.Web.ViewModels.UserGame;

    public class RankingOfSupporterViewModel
    {
        public ICollection<SupporterViewModel> TopFiveSupporters { get; set; }

        public IEnumerable<GamePointsViewModel> TopFiveGame { get; set; }
    }
}
