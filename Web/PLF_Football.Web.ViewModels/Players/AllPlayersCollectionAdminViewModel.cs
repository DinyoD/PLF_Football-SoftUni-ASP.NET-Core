namespace PLF_Football.Web.ViewModels.Players
{
    using System.Collections.Generic;

    public class AllPlayersCollectionAdminViewModel : PagingWithSearchAndSortViewModel
    {
        public IEnumerable<PlayerAdminViewModel> AllPlayers { get; set; }
    }
}
