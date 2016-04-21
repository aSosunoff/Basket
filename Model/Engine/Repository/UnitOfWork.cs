using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Entity;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository
{
    public class UnitOfWork : Engine, IUnitOfWork
    {
        private readonly Entities _entities = new Entities();

        public UnitOfWork()
        {
            Objects.Add(typeof(IBasketRepository), new BasketRepository(_entities));
            Objects.Add(typeof(IProductRepository), new ProductRepository(_entities));
            Objects.Add(typeof(IOrderRepository), new OrderRepository(_entities));
            Objects.Add(typeof(IContractRepository), new ContractRepository(_entities));
            Objects.Add(typeof(ICategoryRepository), new CategoryRepository(_entities));

            Objects.Add(typeof(ITestRepository), new TestRepository(_entities));
        }
    }
}
