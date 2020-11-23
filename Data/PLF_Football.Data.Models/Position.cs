namespace PLF_Football.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Position
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
