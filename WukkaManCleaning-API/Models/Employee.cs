using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WukkaManCleaning_API.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName= "varchar(50)")]
        public string FullName { get; set; }



        [Column(TypeName = "varchar(50)")]
        public string EmailAddress { get; set; }



        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }



        [Column(TypeName = "varchar(15)")]
        public int WorkId { get; set; }
 
    }
}
