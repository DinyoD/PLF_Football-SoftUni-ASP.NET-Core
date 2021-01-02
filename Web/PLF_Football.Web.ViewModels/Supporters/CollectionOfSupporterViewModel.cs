namespace PLF_Football.Web.ViewModels.Supporters
{
    using System.Collections.Generic;

    public class CollectionOfSupporterViewModel : PagingViewModel
    {
        public ICollection<SupporterViewModel> Supporters { get; set; }
    }
}
