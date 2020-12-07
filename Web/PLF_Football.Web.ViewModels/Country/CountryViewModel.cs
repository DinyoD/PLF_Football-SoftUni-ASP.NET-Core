namespace PLF_Football.Web.ViewModels.Country
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class CountryViewModel : IMapFrom<Country>
    {
        public string Name { get; set; }

        public string FlagCode { get; set; }
    }
}
