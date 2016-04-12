using System.Collections.Generic;
using System.Linq;

namespace Model.Infrastructure
{
    public class BasketModels
    {
        public int CountElementToBasket { get; set; }
        public int CountElementToContract { get; set; }
        public IEnumerable<AGRO_PRODUCT> Products { get; set; }

        public decimal ResultAllSum { get { return ProductsToBascet.Sum(x => x.SUM_QANTITY); } }
        public IEnumerable<AGRO_BASKET> ProductsToBascet { get; set; }

    }
}