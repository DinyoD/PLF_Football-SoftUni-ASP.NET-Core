namespace PLF_Football.Web.ViewModels.Players
{
    using System.Collections.Generic;

    using PLF_Football.Web.ViewModels.Clubs;
    using PLF_Football.Web.ViewModels.Position;

    public class AllPlayersCollectionViewModel : PagingWithSearchesViewModel
    {
        public ICollection<PlayerForAllPlayersViewModel> AllPlayers { get; set; }

        public ICollection<PositionBasicViewModel> Positions { get; set; }

        public ICollection<ClubBasicViewModel> Clubs { get; set; }
    }
}
