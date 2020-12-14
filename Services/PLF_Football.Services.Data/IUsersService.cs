namespace PLF_Football.Services.Data
{
    public interface IUsersService
    {
        // void AddPlayerToUserClub(string userId);
        T GetUserById<T>(string userId);
    }
}
