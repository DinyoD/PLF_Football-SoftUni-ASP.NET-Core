namespace PLF_Football.Web.ViewModels.Players
{
    using System.Globalization;

    using PLF_Football.Web.ViewModels.Country;
    using PLF_Football.Web.ViewModels.Position;

    public abstract class PlayerBasicViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName =>
            (this.LastName.Length + (string.IsNullOrEmpty(this.FirstName) ? 0 : this.FirstName.Length)) > 18
            ? this.LastName
            : this.FirstName + " " + this.LastName;

        public string ImageUrl { get; set; }

        public string SquadNumber { get; set; }

        public string PositionName { get; set; }

        public PositionViewModel Position { get; set; }

        public virtual CountryViewModel Country { get; set; }

        public int Price { get; set; }

        public string PriceAtString => this.Price.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
    }
}
