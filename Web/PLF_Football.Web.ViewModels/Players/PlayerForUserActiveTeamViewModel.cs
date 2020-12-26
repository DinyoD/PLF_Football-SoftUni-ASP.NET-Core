namespace PLF_Football.Web.ViewModels.Players
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayerForUserActiveTeamViewModel : IMapFrom<Player>
    {
        public int PositionId { get; set; }

        public int ClubId { get; set; }

        public int Price { get; set; }
    }
}
