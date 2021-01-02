namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClubsService
    {
        ICollection<T> GetAll<T>();

        T GetById<T>(int id);

        string GetClubNameById(int clubId);

        int GetClubIdByClubName(string clubName);
    }
}
