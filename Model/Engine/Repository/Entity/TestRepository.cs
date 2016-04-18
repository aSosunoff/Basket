using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository.Entity
{
    class TestRepository : CRUDRepository<AGRO_TEST, Entities>, ITestRepository
    {
        public TestRepository(Entities entities) : base(entities)
        {
        }
    }
}
