using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage ="field is required")]
        public string Document { get; set; }

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName { get { return string.Format("{0} {1}", FirstName ?? string.Empty, LastName ?? string.Empty).Trim(); } }

        public int DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public object set { get; private set; }
    }
}