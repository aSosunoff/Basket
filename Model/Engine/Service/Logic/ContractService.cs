using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class ContractService : BaseService, IContractService
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
            return _contractRepository.GetAllList().Count();
        }

        public IEnumerable<AGRO_CONTRACT> GetList()
        {
            return _contractRepository.GetAllList();
        }

        public AGRO_CONTRACT GetItemToId(decimal id)
        {
            return _contractRepository.GetItem(e => e.ID == id);
        }
    }
}
