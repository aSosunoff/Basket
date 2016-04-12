using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;
using Model.Engine.Service.Interface;

namespace Model.Engine.Service.Logic
{
    public class BaseService : IBaseService
    {
        public IServiceLayer RootServiceLayer { get; set; }
        public void SetRootService(IServiceLayer serviceLayer)
        {
            RootServiceLayer = serviceLayer;
        }
    }
}
