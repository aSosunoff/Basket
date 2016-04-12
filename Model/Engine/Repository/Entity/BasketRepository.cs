using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class BasketRepository : CRUDRepository<AGRO_BASKET, Entities>, IBasketRepository
    {
        public BasketRepository(Entities entities) : base(entities)
        {
        }
    }
}
