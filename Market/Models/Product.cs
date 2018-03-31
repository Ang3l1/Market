using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime LastBuy { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public int Stock { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


        public virtual ICollection<SupplierProduct> SuppliersProducts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}