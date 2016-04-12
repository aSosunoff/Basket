namespace Model.Engine.Service.Interface
{
    public interface IOrderService : IBaseService
    {
        void Create(AGRO_ORDER item);
    }
}