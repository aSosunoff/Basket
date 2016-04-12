using System.ComponentModel.DataAnnotations;

namespace Model
{
    [MetadataType(typeof(Product))]
    public partial class AGRO_PRODUCT
    {
    }

    public class Product
    {
        [Display(Name = "Наименование")]
        public string NAME { get; set; }
        [Display(Name = "Колличество на скаладе")]
        public decimal QUNTITY { get; set; }
        [Display(Name = "Цена за штуку")]
        public decimal PRICE_ONE { get; set; }
    }
}
