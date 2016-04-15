using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;
using Model.Infrastructure;

namespace Model.Engine.Service.Logic
{
    public class ProductService : BaseService, IProductService
    {
        public IProductRepository ProductRepositoryService { get; set; }

        public ProductService(IUnitOfWork unitOfWork)
        {
            ProductRepositoryService = unitOfWork.Get<IProductRepository>();
        }

        public BasketModels GetBasketModels()
        {
            return new BasketModels()
            {
                CountElementToBasket = RootServiceLayer.Get<IBasketService>().Count(),
                Products = ProductRepositoryService.GetList(),
                CountElementToContract = RootServiceLayer.Get<IContractService>().Count()
            };
        }

        public AGRO_PRODUCT GetItemToId(decimal id)
        {
            return ProductRepositoryService.GetItem(e => e.ID == id);
        }


        public void Update(AGRO_PRODUCT item)
        {
            ProductRepositoryService.Update(item);
        }
    }
}
