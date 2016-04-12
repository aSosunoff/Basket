using System.Collections.Generic;

namespace Model.Engine.Service.Interface
{
    public interface IOrderService : IBaseService
    {
        void Create(AGRO_ORDER item);
        IEnumerable<AGRO_ORDER> GetOrdersByIdContract(decimal id);
    }
}