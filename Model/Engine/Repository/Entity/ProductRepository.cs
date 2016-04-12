using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class ProductRepository : CRUDRepository<AGRO_PRODUCT, Entities>, IProductRepository
    {
        public ProductRepository(Entities entities) : base(entities)
        {
        }
    }
}
