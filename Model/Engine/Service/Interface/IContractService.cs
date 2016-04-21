using System.Collections.Generic;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface IContractService : IBaseService<IContractRepository>
    {
        void Create(AGRO_CONTRACT item);
        int Count();
        IEnumerable<AGRO_CONTRACT> GetList();
        AGRO_CONTRACT GetItemToId(decimal id);
    }
}