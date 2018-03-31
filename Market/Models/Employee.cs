using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="field is required")]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.Time)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "field is required")]
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage ="field is required")]
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Salary { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name ="Bonus %")]
        [Required(AllowEmptyStrings =true)]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public float BonusPercent { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        [Required(ErrorMessage = "field is required")]
        public int DocumentTypeId { get; set; }


        public virtual DocumentType DocumentType { get; set; }



        [NotMapped]
        public int Age { get { return DateTime.Now.Year - DateOfBirth.Year; } }
    }
}