using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class ContractRepository : CRUDRepository<AGRO_CONTRACT, Entities>, IContractRepository
    {
        public ContractRepository(Entities entities) : base(entities)
        {
        }
    }
}
