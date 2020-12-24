namespace PLF_Football.Web.ViewModels.Supporters
{
    using System.Collections.Generic;

    using PLF_Football.Web.ViewModels.UserGame;

    public class CollectionOfSupportersGamePointsViewModel
    {
        public ICollection<GamePointsViewModel> GamesByPoints { get; set; }
    }
}
