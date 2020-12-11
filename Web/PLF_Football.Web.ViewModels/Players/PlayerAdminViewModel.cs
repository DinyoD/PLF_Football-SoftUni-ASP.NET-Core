namespace PLF_Football.Web.ViewModels.Players
{
    using System.ComponentModel.DataAnnotations;

    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PlayerAdminViewModel : IMapFrom<Player>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ClubName { get; set; }

        [Range(200_000, 8_000_000)]
        public int Price { get; set; }
    }
}
