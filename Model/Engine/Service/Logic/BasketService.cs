﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace Model.Engine.Service.Logic
{
    public class BasketService : BaseService, IBasketService
    {
        public IBasketRepository BasketRepositoryService { get; set; }

        public BasketService(IUnitOfWork unitOfWork)
        {
            BasketRepositoryService = unitOfWork.Get<IBasketRepository>();
        }

        public int Count()
        {
            return BasketRepositoryService.GetList().Count();
        }

        public void AddedProductToBasket(AGRO_BASKET productToBasket)
        {
            if (productToBasket.QANTITY > RootServiceLayer.Get<IProductService>().GetItemToId(productToBasket.ID_PRODUCT).QUNTITY)
                throw new Exception("Нельзя добавить такое колличество товара");

            var prod = BasketRepositoryService.GetList().SingleOrDefault(x => x.ID_PRODUCT == productToBasket.ID_PRODUCT);
            
            //проверяем есть ли такой товар в корзине
            if (prod == null)
            {
                productToBasket.DATA_START = DateTime.Now;
                BasketRepositoryService.Create(productToBasket);
            }
            else
            {
                prod.QANTITY += productToBasket.QANTITY;

                if (prod.QANTITY > RootServiceLayer.Get<IProductService>().GetItemToId(productToBasket.ID_PRODUCT).QUNTITY)
                    throw new Exception("Нельзя добавить такое колличество товара");

                prod.DATA_START = DateTime.Now;

                BasketRepositoryService.Update(prod);
            }
        }

        public BasketModels GetBasketModels()
        {
            //проверяем есть ли добавленый ранее товар в корзине на складе
            //если нет то выводим предупреждающее сообщение
            string errorMessage;
            if (IsNullQantityProduct(out errorMessage))
                throw new Exception(errorMessage);

            return new BasketModels()
            {
                ProductsToBascet = BasketRepositoryService.GetList()
            };
        }

        private bool IsNullQantityProduct(out string errorMessage)
        {
            bool errorFlag = false;
            errorMessage = String.Empty;

            foreach (var element in BasketRepositoryService.GetList())
            {
                if (RootServiceLayer.Get<IProductService>().GetItemToId(element.ID_PRODUCT).QUNTITY < element.QANTITY)
                {
                    errorFlag = true;
                    errorMessage += String.Format("Продукта \"{0}\" нет на складе\n", element.AGRO_PRODUCT.NAME);
                }
                    
            }
            return errorFlag;
        }

        public void Order()
        {
            //проверяем есть ли добавленый ранее товар в корзине на складе
            //если нет то выводим предупреждающее сообщение
            string errorMessage;
            if(IsNullQantityProduct(out errorMessage))
                throw new Exception(errorMessage);
            //создаём контракт который прикрепим к заказу
            //добавляем информацию о клиенте и о поставщике
            //продумать индификатор поставщика
            AGRO_CONTRACT contract = new AGRO_CONTRACT(){ DATE_START = DateTime.Now };

            RootServiceLayer.Get<IContractService>().Create(contract);

            //создаём заказ. и перемещаем товары из корзины в заказ
            foreach (var element in BasketRepositoryService.GetList())
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
                BasketRepositoryService.Delete(element);
            }
        }
    }
}
