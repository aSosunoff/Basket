using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    class TestService : BaseService, ITestService
    {
        public ITestRepository TestRepository { get; set; }

        public TestService(IUnitOfWork unitOfWork)
        {
            TestRepository = unitOfWork.Get<ITestRepository>();
        }
    }
}
