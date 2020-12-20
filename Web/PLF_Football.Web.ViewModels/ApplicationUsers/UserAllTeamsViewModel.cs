namespace PLF_Football.Web.ViewModels.ApplicationUsers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.UserGame;

    public class UserAllTeamsViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int FavoriteTeamId { get; set; }

        public string FavoriteTeamName { get; set; }

        public ICollection<UserGameTeamViewModel> Teams { get; set; }

        public int TotalPoints => this.Teams.Sum(x => x.Points);

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserAllTeamsViewModel>()
                 .ForMember(x => x.Teams, op => op.MapFrom(x => x.Games));
        }
    }
}
