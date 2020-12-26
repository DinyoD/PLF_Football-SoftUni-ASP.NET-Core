namespace PLF_Football.Data.Models
{
    using PLF_Football.Data.Common.Models;

    public class Message : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Text { get; set; }
    }
}
