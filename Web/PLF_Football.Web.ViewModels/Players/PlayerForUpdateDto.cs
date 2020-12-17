namespace PLF_Football.Web.ViewModels.Players
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayerForUpdateDto : IMapFrom<Player>
    {
        public int Id { get; set; }

        public string SquadNumber { get; set; }

        public int ClubId { get; set; }

        public PlayerStatsForUpdateDto PlayerStats { get; set; }

        public int Points { get; set; }
    }
}
