namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClubsService
    {
        IEnumerable<T> GetAll<T>();

        T GetById<T>(int id);
    }
}
