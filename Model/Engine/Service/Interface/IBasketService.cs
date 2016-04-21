using System.Collections.Generic;
using Model.Engine.Repository.Interface;
using Model.Infrastructure;

namespace Model.Engine.Service.Interface
{
    public interface IBasketService : IBaseService<IBasketRepository>
    {
        int Count();
        void AddedProductToBasket(AGRO_BASKET productToBasket);
        BasketModels GetBasketModels();
        void Order();
    }
}