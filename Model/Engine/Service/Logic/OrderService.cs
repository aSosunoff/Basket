using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class OrderService : BaseService<IOrderRepository>, IOrderService
    {

        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork){}

        public void Create(AGRO_ORDER item)
        {
            _Repository.Create(item);
        }

        public IEnumerable<AGRO_ORDER> GetOrdersByIdContract(decimal id)
        {
            return _Repository.GetAllList().Where(e => e.ID_CONTRACT == id);
        }

        
    }
}
