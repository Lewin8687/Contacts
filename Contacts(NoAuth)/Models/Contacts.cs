using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Contacts_NoAuth_.Models
{
    public class Contacts
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Format invalid!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Postal code is required!")]
        [RegularExpression(@"^[A-Za-z]\d{1}[A-Za-z]\d{1}[A-Za-z]\d{1}$", ErrorMessage = "Format invalid!")]
        public string Postal { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(200)]
        public string Address { get; set; }

        public bool IsDeleted { get; set; }

        public Contacts()
        {
            IsDeleted = false;
        }
    }
}