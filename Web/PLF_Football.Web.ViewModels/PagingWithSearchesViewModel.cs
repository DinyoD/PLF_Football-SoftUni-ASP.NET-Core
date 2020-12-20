namespace PLF_Football.Web.ViewModels
{
    public class PagingWithSearchesViewModel : PagingViewModel
    {
        public int PositionSearch { get; set; }

        public int ClubSearch { get; set; }

        public string SearchString { get; set; }
    }
}
