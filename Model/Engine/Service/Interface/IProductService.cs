using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Model.Infrastructure;

namespace Model.Engine.Service.Interface
{
    public interface IProductService : IBaseService
    {
        BasketModels GetBasketModels();
        AGRO_PRODUCT GetItemToId(decimal id);
        void Create(AGRO_PRODUCT item);
        void Update(AGRO_PRODUCT item);
    }
}