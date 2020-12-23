namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;

    public interface IUsersService
    {
        // void AddPlayerToUserClub(string userId);
        T GetUserById<T>(string userId);

        ICollection<T> GetAllUsers<T>();
    }
}
