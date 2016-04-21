using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Infrastructure
{
    public class ModelCatalog
    {
        public IEnumerable<AGRO_CATEGORY> Categorys { get; set; }
        public IEnumerable<AGRO_PRODUCT> Products { get; set; } 
    }
}
