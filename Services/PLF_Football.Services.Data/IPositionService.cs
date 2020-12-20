namespace PLF_Football.Services.Data
{
    using System.Collections.Generic;

    public interface IPositionService
    {
        ICollection<T> GetAll<T>();
    }
}
