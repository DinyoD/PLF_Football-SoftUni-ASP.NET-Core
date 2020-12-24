namespace PLF_Football.Web.ViewModels.UserGame
{
    using System.Globalization;
    using System.Linq;

    using AutoMapper;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class GamePointsViewModel : IMapFrom<UserGame>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public string UserFavoriteTeamBadgeUrl { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }

        public int TeamSum { get; set; }

        public string TeamSumAtString => this.TeamSum.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserGame, GamePointsViewModel>()
                .ForMember(x => x.TeamSum, op => op.MapFrom(x => x.MatchdayTeam.Sum(x => x.Player.Price)));
        }
    }
}
