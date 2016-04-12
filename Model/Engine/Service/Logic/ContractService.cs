using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    class ContractService : BaseService, IContractService
    {
        //todo: приходиться всё время копировать. Придумать автоматическую генерацию
        private IContractRepository _contractRepository;

        public ContractService(IUnitOfWork unitOfWork)
        {
            _contractRepository = unitOfWork.Get<IContractRepository>();
        }

        public void Create(AGRO_CONTRACT item)
        {
            _contractRepository.Create(item);
        }

        public int Count()
        {
            return _contractRepository.GetList().Count();
        }
    }
}
