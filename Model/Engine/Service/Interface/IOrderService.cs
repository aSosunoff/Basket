using System.Collections.Generic;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface IOrderService : IBaseService<IOrderRepository>
    {
        void Create(AGRO_ORDER item);
        IEnumerable<AGRO_ORDER> GetOrdersByIdContract(decimal id);
    }
}