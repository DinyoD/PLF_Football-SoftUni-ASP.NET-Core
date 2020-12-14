namespace PLF_Football.Services.Data
{
    using System.Linq;

    using PLF_Football.Data.Common.Repositories;
    using PLF_Football.Data.Models;
    using PLF_Football.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepo)
        {
            this.userRepo = userRepo;
        }

        // public void AddPlayerToUserClub(string userId)
        // {
        // }
        public T GetUserById<T>(string userId)
        {
            return this.userRepo.AllAsNoTracking().Where(x => x.Id == userId).To<T>().FirstOrDefault();
        }
    }
}
