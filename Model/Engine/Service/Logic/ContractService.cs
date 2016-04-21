using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class ContractService : BaseService<IContractRepository>, IContractService
    {
        public ContractService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public void Create(AGRO_CONTRACT item)
        {
            _Repository.Create(item);
        }

        public int Count()
        {
            return _Repository.GetAllList().Count();
        }

        public IEnumerable<AGRO_CONTRACT> GetList()
        {
            return _Repository.GetAllList();
        }

        public AGRO_CONTRACT GetItemToId(decimal id)
        {
            return _Repository.GetItem(e => e.ID == id);
        }
    }
}
