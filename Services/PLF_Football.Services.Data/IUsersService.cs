namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;

    public interface IUsersService
    {
        T GetUserById<T>(string userId);

        ICollection<T> GetAllUsers<T>();

        int GetFavoriteClubIdByUserId(string userId);
    }
}
