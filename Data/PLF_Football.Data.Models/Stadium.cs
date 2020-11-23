namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;

    public class Stadium : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Info { get; set; }

        public string ImageUrl { get; set; }
    }
}
