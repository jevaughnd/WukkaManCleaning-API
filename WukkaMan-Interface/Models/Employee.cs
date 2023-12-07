using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace WukkaMan_Interface.Models
{
    public class Employee
    {
        public int Id { get; set; }


        [Display(Name = "Full Name")]
        public string FullName { get; set; }



        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }



        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }



        [Display(Name = "Work ID")]
        public int WorkId { get; set; }
    }
}
