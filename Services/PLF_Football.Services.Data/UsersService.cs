namespace PLF_Football.Services.Data
{
    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using System.Linq;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepo)
        {
            this.userRepo = userRepo;
        }

        public void AddPlayerToUserClub(string userId)
        {
        }
    }
}
