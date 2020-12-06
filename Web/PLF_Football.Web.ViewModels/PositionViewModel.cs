namespace PLF_Football.Web.ViewModels
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PositionViewModel : IMapFrom<Position>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
