namespace PLF_Football.Web.ViewModels
{
    public class PagingWithSearchAndSortViewModel : PagingViewModel
    {
        public string SearchString { get; set; }

        public string SortOrder { get; set; }

        public string CurrentFilter { get; set; }
    }
}
