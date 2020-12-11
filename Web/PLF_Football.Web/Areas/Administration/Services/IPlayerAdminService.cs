namespace PLF_Football.Web.Areas.Administration.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlayerAdminService
    {
        ICollection<T> GetAll<T>();

        T GetPlayerbyId<T>(int? id);

        Task UpdatePlayerPrice(int id, int price);
    }
}
