using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace Model.Engine.Service.Logic
{
    public class BasketService : BaseService, IBasketService
    {
        private IBasketRepository _basketRepository;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _basketRepository = unitOfWork.Get<IBasketRepository>();
        }

        public int Count()
        {
            return _basketRepository.GetList().Count();
        }

        public void AddedProductToBasket(AGRO_BASKET productToBasket)
        {
            if (productToBasket.QANTITY > RootServiceLayer.Get<IProductService>().GetItemToId(productToBasket.ID_PRODUCT).QUNTITY)
                throw new Exception("Нельзя добавить такое колличество товара");

            var prod = _basketRepository.GetList().SingleOrDefault(x => x.ID_PRODUCT == productToBasket.ID_PRODUCT);// _basketRepository.GetItem(x => x.ID_PRODUCT == productToBasket.ID_PRODUCT && x.ORDER_FLAG == 0);//AGRO_BASKET.SingleOrDefault();
            
            //проверяем есть ли такой товар в корзине
            if (prod == null)
            {
                productToBasket.DATA_START = DateTime.Now;
                _basketRepository.Create(productToBasket);
            }
            else
            {
                prod.QANTITY += productToBasket.QANTITY;

                if (prod.QANTITY > RootServiceLayer.Get<IProductService>().GetItemToId(productToBasket.ID_PRODUCT).QUNTITY)
                    throw new Exception("Нельзя добавить такое колличество товара");

                prod.DATA_START = DateTime.Now;

                _basketRepository.Update(prod);
            }
        }

        public BasketModels GetBasketModels()
        {
            return new BasketModels()
            {
                ProductsToBascet = _basketRepository.GetList()
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
            foreach (var element in _basketRepository.GetList())
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
                _basketRepository.Delete(element);
            }
            

            //foreach (var element in GetListBasket())
            //{
            //    element.ID_ORDER = agroOrder.ID;
                
            //    element.ORDER_FLAG = 1;

            //    _basketRepository.Update(element);
            //}
        }
    }
}
