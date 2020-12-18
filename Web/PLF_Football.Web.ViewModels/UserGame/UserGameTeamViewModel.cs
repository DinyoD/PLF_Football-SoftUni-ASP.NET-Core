namespace PLF_Football.Web.ViewModels.UserGame
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AutoMapper;
    using PLF_Football.Common;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Players;

    public class UserGameTeamViewModel : IMapFrom<UserGame>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int Matchday { get; set; }

        public bool IsMatchdayStarted { get; set; }

        public int Points => this.Team
                                        .Sum(x => x.PlayerPoints
                                                    .Where(y => y.Matchday == this.Matchday)
                                                    .Select(z => z.Points)
                                                    .FirstOrDefault());

        public virtual ICollection<UserTeamPlayerViewModel> Team { get; set; }

        public int TeamSum => this.Team.Sum(x => x.Price);

        public string TeamSumAsString => this.TeamSum.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));

        public int FreeBudget => GlobalConstants.UserBudget - this.TeamSum;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserGame, UserGameTeamViewModel>()
                .ForMember(x => x.Team, op => op.MapFrom(x => x.MatchdayTeam.Select(y => y.Player)));
        }
    }
}
