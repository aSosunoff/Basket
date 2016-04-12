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
        private IProductRepository _productRepository;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _productRepository = unitOfWork.Get<IProductRepository>();
        }

        public BasketModels GetBasketModels()
        {
            return new BasketModels()
            {
                CountElementToBasket = RootServiceLayer.Get<IBasketService>().Count(),
                Products = _productRepository.GetList(),
                CountElementToContract = RootServiceLayer.Get<IContractService>().Count()
            };
        }

        public AGRO_PRODUCT GetItemToId(decimal id)
        {
            return _productRepository.GetItem(e => e.ID == id);
        }

        public void Create(AGRO_PRODUCT item)
        {
            //todo: подумать как можно не печатая вынести медод из UnitOfWork
            _productRepository.Update(item);
        }

        public void Update(AGRO_PRODUCT item)
        {
            _productRepository.Update(item);
        }
    }
}
