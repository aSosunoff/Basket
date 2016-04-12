using System.Collections.Generic;

namespace Model.Engine.Service.Interface
{
    public interface IContractService : IBaseService
    {
        void Create(AGRO_CONTRACT item);
        int Count();
        IEnumerable<AGRO_CONTRACT> GetList();
        AGRO_CONTRACT GetItemToId(decimal id);
    }
}