namespace Model.Engine.Service.Interface
{
    public interface IContractService : IBaseService
    {
        void Create(AGRO_CONTRACT item);
        int Count();
    }
}