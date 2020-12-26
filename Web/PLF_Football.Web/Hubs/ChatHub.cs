namespace PLF_Football.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUsersService usersService;
        private readonly IClubsService clubsService;
        private readonly IMessagesService messagesService;

        public ChatHub(
            IUsersService usersService,
            IClubsService clubsService,
            IMessagesService messagesService)
        {
            this.usersService = usersService;
            this.clubsService = clubsService;
            this.messagesService = messagesService;
        }

        public async Task Send(string message)
        {
            var user = this.Context.User.Identity.Name;
            var userId = this.Context.UserIdentifier;
            var clubId = this.usersService.GetFavoriteClubIdByUserId(userId);
            var clubName = this.clubsService.GetClubNameById(clubId);

            await this.messagesService.AddMessageAssyns(userId, message);

            await this.Clients.All.SendAsync(
                "NewMessage",
                new Message
                {
                    Name = user,
                    Text = message,
                    Club = clubName,
                });
        }
    }
}
