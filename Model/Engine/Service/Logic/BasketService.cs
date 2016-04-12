using System;
using System.Collections.Generic;
using System.Linq;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace Model.Engine.Service.Logic
{
    public class BasketService : BaseService, IBasketService
    {
        public IBasketRepository BasketRepository { get; set; }

        public BasketService(IUnitOfWork unitOfWork)
        {
            BasketRepository = unitOfWork.Get<IBasketRepository>();
        }

        public int Count()
        {
            return BasketRepository.GetList().Count();
        }

        public void AddedProductToBasket(AGRO_BASKET productToBasket)
        {
            if (productToBasket.QANTITY > RootServiceLayer.Get<IProductService>().GetItemToId(productToBasket.ID_PRODUCT).QUNTITY)
                throw new Exception("Нельзя добавить такое колличество товара");

            var prod = BasketRepository.GetList().SingleOrDefault(x => x.ID_PRODUCT == productToBasket.ID_PRODUCT);
            
            //проверяем есть ли такой товар в корзине
            if (prod == null)
            {
                productToBasket.DATA_START = DateTime.Now;
                BasketRepository.Create(productToBasket);
            }
            else
            {
                prod.QANTITY += productToBasket.QANTITY;

                if (prod.QANTITY > RootServiceLayer.Get<IProductService>().GetItemToId(productToBasket.ID_PRODUCT).QUNTITY)
                    throw new Exception("Нельзя добавить такое колличество товара");

                prod.DATA_START = DateTime.Now;

                BasketRepository.Update(prod);
            }
        }

        public BasketModels GetBasketModels()
        {
            return new BasketModels()
            {
                ProductsToBascet = BasketRepository.GetList()
            };
        }

        public void Order()
        {
            //создаём контракт который прикрепим к заказу
            //добавляем информацию о клиенте и о поставщике
            //продумать индификатор поставщика
            AGRO_CONTRACT contract = new AGRO_CONTRACT(){ DATE_START = DateTime.Now };

            RootServiceLayer.Get<IContractService>().Create(contract);

            //создаём заказ. и перемещаем товары из корзины в заказ
            foreach (var element in BasketRepository.GetList())
            {
                AGRO_ORDER order = new AGRO_ORDER()
                {
                    ID_CONTRACT = contract.ID,
                    NAME = element.AGRO_PRODUCT.NAME,
                    PRICE_ONE = element.AGRO_PRODUCT.PRICE_ONE,
                    QANTITY = element.QANTITY
                };

                RootServiceLayer.Get<IOrderService>().Create(order);

                //вычитаем из склада
                //предусмотреть проверку на остаток на складе
                //недопустить что бы на складе вышло колличество в минус
                RootServiceLayer.Get<IProductService>().GetItemToId(element.ID_PRODUCT).QUNTITY -= element.QANTITY;
                RootServiceLayer.Get<IProductService>().Update(RootServiceLayer.Get<IProductService>().GetItemToId(element.ID_PRODUCT));
                
                //удаляем из корзины
                BasketRepository.Delete(element);
            }
        }
    }
}
