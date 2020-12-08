namespace PLF_Football.Web.ViewModels.UserGame
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Players;

    public class UserGameTeamViewModel : IMapFrom<UserGame>, IHaveCustomMappings
    {
        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public string UserFavoriteTeamName { get; set; }

        public int UserTotalPoints { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }

        public virtual ICollection<UserTeamPlayerViewModel> MatchdayTeam { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserGame, UserGameTeamViewModel>()
                .ForMember(x => x.MatchdayTeam, op => op.MapFrom(x => x.MatchdayTeam.Select(y => y.Player)));
        }
    }
}
