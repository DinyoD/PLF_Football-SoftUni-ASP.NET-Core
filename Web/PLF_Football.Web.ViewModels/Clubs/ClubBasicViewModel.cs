namespace PLF_Football.Web.ViewModels.Clubs
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class ClubBasicViewModel : IMapFrom<Club>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
