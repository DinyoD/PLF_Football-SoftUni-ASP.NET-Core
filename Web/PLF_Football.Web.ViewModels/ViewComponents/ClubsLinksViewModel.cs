namespace PLF_Football.Web.ViewModels.ViewComponents
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class ClubsLinksViewModel : IMapFrom<Club>
    {
        public int Id { get; set; }

        public string BadgeUrl { get; set; }
    }
}
