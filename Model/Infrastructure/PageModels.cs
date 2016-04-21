using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Service;
using Model.Engine.Service.Interface;
using Model.Engine.Service.Logic;

namespace Model.Infrastructure
{
    public class PageModels
    {
        private IServiceLayer _serviceLayer { get; set; }
        public PageModels(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);
        }

        /// <summary>
        /// Колличество продуктов в корзине
        /// </summary>
        public int CountElementToBasket
        {
            set { }
            get { return _serviceLayer.Get<IBasketService>().Count(); } 
        }
        /// <summary>
        /// Колличество оформленных заказов
        /// </summary>
        public int CountElementToContract
        {
            set { }
            get { return _serviceLayer.Get<IContractService>().Count(); }    
        }


        /// <summary>
        /// Товары которые лежат в корзине
        /// </summary>
        public IEnumerable<AGRO_BASKET> ProductsToBascet
        {
            get
            {
                //проверяем есть ли добавленый ранее товар в корзину на складе
                //если нет то выводим предупреждающее сообщение            

                return _serviceLayer.Get<IBasketService>()._Repository.GetAllList();
            } 
            set{}
        }
        public void IsProductsToBascet()
        {
            try
            {
                string errorMessage;
                if (IsNullQantityProduct(out errorMessage))
                    throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Итоговая сумма товаров которые лежат в корзине
        public decimal ResultAllSumToBasket { get { return ProductsToBascet.Sum(x => x.SUM_QANTITY); } }

        //Итоговая сумма товаров которые заказаны
        public decimal ResultAllSumToOrder { get { return Orders.Sum(x => x.PRICE_ONE * x.QANTITY); } }

        public IEnumerable<AGRO_CONTRACT> Contracts
        {
            get { return _serviceLayer.Get<IContractService>().GetList(); }
            set { }
        }

        public IEnumerable<AGRO_ORDER> Orders { get; set; }

        public AGRO_PRODUCT Product { get; set; }
        public AGRO_BASKET Basket { get; set; }
        public AGRO_ORDER Order { get; set; }

        /// <summary>
        /// Поле сообщения об ошибки. Выводиться на главной страницы просмотра
        /// </summary>
        public string ErrorMessage { get; set; }

        







        private bool IsNullQantityProduct(out string errorMessage)
        {
            bool errorFlag = false;
            errorMessage = String.Empty;

            foreach (var element in _serviceLayer.Get<IBasketService>()._Repository.GetAllList())
            {
                if (_serviceLayer.Get<IProductService>().GetItemToId(element.ID_PRODUCT).QUNTITY < element.QANTITY)
                {
                    errorFlag = true;
                    errorMessage += String.Format("Продукта \"{0}\" нет на складе\n", element.AGRO_PRODUCT.NAME);
                }
                    
            }
            return errorFlag;
        }
    }
}
