namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;

    public class PlayerUserGame : BaseModel<int>
    {
        public int PlayerId { get; set; }

        public Player Player { get; set; }

        public int UserGameId { get; set; }

        public UserGame UserGame { get; set; }
    }
}
