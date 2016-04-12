using System.ComponentModel.DataAnnotations;

namespace Model
{
    [MetadataType(typeof(Basket))]
    public partial class AGRO_BASKET
    {
        [Display(Name = "Сумма")]
        public decimal SUM_QANTITY { 
            get
            {
                decimal productPriceOne = this.AGRO_PRODUCT != null ? this.AGRO_PRODUCT.PRICE_ONE : 1;
                return this.QANTITY * productPriceOne;
            }}
    }

    public class Basket
    {
        [Required(ErrorMessage = "Необходимо заполнить колличество")]
        [Display(Name = "Колличество")]
        public decimal QANTITY { get; set; }

        [Display(Name = "Дата добавления")]
        public System.DateTime DATA_START { get; set; }


    }
}
