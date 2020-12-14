namespace PLF_Football.Web.ViewModels.ApplicationUsers
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int FavoriteTeamId { get; set; }

        public string FavoriteTeamName { get; set; }
    }
}
