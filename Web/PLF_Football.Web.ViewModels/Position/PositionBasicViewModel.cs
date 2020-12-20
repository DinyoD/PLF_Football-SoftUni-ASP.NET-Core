namespace PLF_Football.Web.ViewModels.Position
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class PositionBasicViewModel : IMapFrom<Position>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
