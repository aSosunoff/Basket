using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Model.Engine.Repository.Interface;
using Model.Infrastructure;

namespace Model.Engine.Service.Interface
{
    public interface IProductService : IBaseService
    {
        IProductRepository ProductRepositoryService { get; set; }
        BasketModels GetBasketModels();
        AGRO_PRODUCT GetItemToId(decimal id);
        void Update(AGRO_PRODUCT item);
    }
}