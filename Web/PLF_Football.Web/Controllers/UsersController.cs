namespace PLF_Football.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        public UsersController()
        {

        }

        public IActionResult Team(int id)
        {
            return this.View();
        }
    }
}
