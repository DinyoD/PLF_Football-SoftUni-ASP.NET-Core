namespace PLF_Football.Web.ViewModels.ApplicationUsers
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class SupporterViewModel : IMapFrom<ApplicationUser>
    {
        public string Name { get; set; }

        public int GamesPointsSum { get; set; }
    }
}
