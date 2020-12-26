namespace PLF_Football.Web.ViewModels.Messages
{
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class MessageViewModel : IMapFrom<Message>
    {
        public int Id { get; set; }

        public string UserUserName { get; set; }

        public string Text { get; set; }

        public string UserFavoriteTeamName { get; set; }
    }
}
