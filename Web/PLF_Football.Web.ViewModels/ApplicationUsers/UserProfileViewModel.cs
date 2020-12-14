namespace PLF_Football.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.UserGame;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int FavoriteTeamId { get; set; }

        public string FavoriteTeamName { get; set; }

        public ICollection<UserGameTeamViewModel> Teams { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserProfileViewModel>()
                 .ForMember(x => x.Teams, op => op.MapFrom(x => x.Games));
        }
    }
}
