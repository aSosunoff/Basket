using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface ITestService : IBaseService
    {
        ITestRepository TestRepository { get; set; }
    }
}