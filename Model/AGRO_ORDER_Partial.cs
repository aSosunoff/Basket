using System.ComponentModel.DataAnnotations;

namespace Model
{
    [MetadataType(typeof(Order))]
    public partial class AGRO_ORDER
    {
        //[Display(Name = "Сумма")]
        //public decimal SUM_QANTITY
        //{
        //    get
        //    {
        //        decimal productPriceOne = this.AGRO_CONTRACT != null ? this.AGRO_CONTRACT.PRICE_ONE : 1;
        //        return this.QANTITY * productPriceOne;
        //    }
        //}
    }

    class Order
    {
         
    }
}
