using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace AGRO.Component
{
    public class StartViewBag
    {
        public IServiceLayer _serviceLayer { get; set; }
        public StartViewBag(IServiceLayer serviceLayer)
        {
            _serviceLayer = ServiceLayer.Instance(serviceLayer);

            CountElementToBasket = _serviceLayer.Get<IBasketService>().Count();
            CountElementToContract = _serviceLayer.Get<IContractService>().Count();

            ConnectByPriorInModel model = new ConnectByPriorInModel()
            {
                StartWith = new StartWith()
                {
                    ColummName = "ID",
                    ColummValue = 0
                },
                ConnectByPrior = new ConnectByPrior()
                {
                    Left = "ID",
                    Right = "P_ID"
                }
            };

            WrapModels = _serviceLayer.Get<ICategoryService>()._Repository.GetAllList().ConnectByPriorAllElement(model);



        }
        public List<WrapModel<AGRO_CATEGORY>> WrapModels { get; set; }

        /// <summary>
        /// Колличество продуктов в корзине
        /// </summary>
        public int CountElementToBasket { get; set; }
        /// <summary>
        /// Колличество оформленных заказов
        /// </summary>
        public int CountElementToContract { get; set; }
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