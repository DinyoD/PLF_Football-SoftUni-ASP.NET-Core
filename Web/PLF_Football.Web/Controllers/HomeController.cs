namespace PLF_Football.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PLF_Football.Services.Data;
    using PLF_Football.Web.ViewModels;
    using PLF_Football.Web.ViewModels.Messages;

    public class HomeController : BaseController
    {
        private readonly IMessagesService messagesService;

        public HomeController(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Chat()
        {
            var lastHundredMessages = this.messagesService.GetLastHundred<MessageViewModel>();
            var viewModel = new AllMessagesViewModel
            {
                AllMessages = lastHundredMessages,
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult GameRules()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
