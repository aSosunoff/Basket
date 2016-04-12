using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    class OrderService : BaseService, IOrderService
    {
        private IOrderRepository _orderRepository;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _orderRepository = unitOfWork.Get<IOrderRepository>();
        }

        public void Create(AGRO_ORDER item)
        {
            _orderRepository.Create(item);
        }

        public IEnumerable<AGRO_ORDER> GetOrdersByIdContract(decimal id)
        {
            return _orderRepository.GetList().Where(e => e.ID_CONTRACT == id);
        }
    }
}
