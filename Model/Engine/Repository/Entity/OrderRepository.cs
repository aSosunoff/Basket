using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class OrderRepository : CRUDRepository<AGRO_ORDER, Entities>, IOrderRepository
    {
        public OrderRepository(Entities entities) : base(entities)
        {
        }
    }
}
