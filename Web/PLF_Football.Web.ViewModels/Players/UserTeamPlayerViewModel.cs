namespace PLF_Football.Web.ViewModels.Players
{
    using System.Collections.Generic;
    using System.Globalization;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;
    using PLF_Football.Web.ViewModels.Country;
    using PLF_Football.Web.ViewModels.PlayersPoints;
    using PLF_Football.Web.ViewModels.Position;

    public class UserTeamPlayerViewModel : IMapFrom<Player>
    {
        public int Id { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }

        public string PositionName { get; set; }

        public PositionBasicViewModel Position { get; set; }

        public int Price { get; set; }

        public string PriceAtString => this.Price.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));

        public ICollection<PlayerPointsByFixtureViewModel> PlayerPoints { get; set; }
    }
}
