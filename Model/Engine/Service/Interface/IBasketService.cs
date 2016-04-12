using System.Collections.Generic;
using Model.Infrastructure;

namespace Model.Engine.Service.Interface
{
    public interface IBasketService : IBaseService
    {
        int Count();
        void AddedProductToBasket(AGRO_BASKET productToBasket);
        BasketModels GetBasketModels();
        void Order();
    }
}