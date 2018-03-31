using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        
        public string Address { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<SupplierProduct> SuppliersProducts { get; set; }
    }
}