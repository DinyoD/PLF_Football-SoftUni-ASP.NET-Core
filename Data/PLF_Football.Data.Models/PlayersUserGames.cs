namespace PLF_Football.Data.Models
{
    public class PlayersUserGames
    {
        public int PlayerId { get; set; }

        public Player Player { get; set; }

        public int UserGameId { get; set; }

        public UserGame UserGame { get; set; }
    }
}
