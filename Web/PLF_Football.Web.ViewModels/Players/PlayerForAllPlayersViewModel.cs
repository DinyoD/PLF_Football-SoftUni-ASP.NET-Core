namespace PLF_Football.Web.ViewModels.Players
{
    using System.Globalization;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Clubs;
    using PLF_Football.Web.ViewModels.Position;

    public class PlayerForAllPlayersViewModel : IMapFrom<Player>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => this.FirstName + " " + this.LastName;

        public string ImageUrl { get; set; }

        public ClubBasicViewModel Club { get; set; }

        public PositionBasicViewModel Position { get; set; }

        public int Price { get; set; }

        public string PriceAtString => this.Price.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
    }
}
