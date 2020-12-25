namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
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

        public ICollection<T> GetAllUsers<T>()
        {
            return this.userRepo.AllAsNoTracking().Where(x => x.FavoriteTeam != null).To<T>().ToList();
        }

        public int GetFavoriteClubIdByUserId(string userId)
        {
            var user = this.userRepo.AllAsNoTracking().Where(x => x.Id == userId).FirstOrDefault();
            if (user.FavoriteTeam != null)
            {
                return user.FavoriteTeam.Id;
            }

            return -1;
        }

        public T GetUserById<T>(string userId)
        {
            return this.userRepo.AllAsNoTracking().Where(x => x.Id == userId).To<T>().FirstOrDefault();
        }
    }
}
