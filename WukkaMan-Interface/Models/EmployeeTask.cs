using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace WukkaMan_Interface.Models
{
    public class EmployeeTask
    {
        public int Id { get; set; }


        //fk
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }



        //fk
        [Display(Name = "Task")]
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual EmpTask? Task { get; set; }
     



        [Display(Name = "Is Completed")]
        public bool isCompleted { get; set; }


        [Display(Name = "Assigment Date")]
        public DateTime AssigmentDate { get; set; }
    }
}
