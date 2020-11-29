namespace PLF_Football.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using PLF_Football.Data.Common.Models;

    public class Stadium : BaseModel<int>
    {
        public string Name { get; set; }

        public string Info { get; set; }

        public string ImageUrl { get; set; }

        public int ClubId { get; set; }

        [ForeignKey("ClubId")]
        public virtual Club Club { get; set; }
    }
}
