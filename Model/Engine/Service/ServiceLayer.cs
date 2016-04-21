using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;
using Model.Engine.Service.Logic;

namespace Model.Engine.Service
{
    public class ServiceLayer : Engine, IServiceLayer
    {
        public static IServiceLayer Instance(IServiceLayer serviceLayer)
        {
            serviceLayer.Get<IBasketService>().SetRootService(serviceLayer);
            serviceLayer.Get<IProductService>().SetRootService(serviceLayer);
            serviceLayer.Get<IOrderService>().SetRootService(serviceLayer);
            serviceLayer.Get<IContractService>().SetRootService(serviceLayer);
            serviceLayer.Get<ICategoryService>().SetRootService(serviceLayer);

            serviceLayer.Get<ITestService>().SetRootService(serviceLayer);
            return serviceLayer;
        }
        public ServiceLayer(IUnitOfWork unitOfWork)
        {
            Objects.Add(typeof(IBasketService), new BasketService(unitOfWork));
            Objects.Add(typeof(IProductService), new ProductService(unitOfWork));
            Objects.Add(typeof(IOrderService), new OrderService(unitOfWork));
            Objects.Add(typeof(IContractService), new ContractService(unitOfWork));
            Objects.Add(typeof(ICategoryService), new CategoryService(unitOfWork));

            Objects.Add(typeof(ITestService), new TestService(unitOfWork));
        }
    }
}
