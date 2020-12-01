namespace PLF_Football.Services.Data.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClubsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
