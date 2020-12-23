namespace PLF_Football.Web.ViewModels.UserGame
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class GamePointsViewModel : IMapFrom<UserGame>
    {
        public int Id { get; set; }

        public int Matchday { get; set; }

        public int Points { get; set; }
    }
}
