using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WukkaMan_Interface.Models.ViewModel
{
    public class TaskVM
    {
        public int Id { get; set; }

        //fk
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        //values
        public List<SelectListItem>? EmployeeList { get; set; }
        
        //Selected DDL Value
        public int SelectedEployeeId { get; set; }

        //fk
        [Display(Name = "Task")]
        public int TaskId { get; set; }

        //Values
        public List<SelectListItem>? TaskList { get; set; }


        //Selected DDL Value
        public int SelectedTaskId { get; set; }


        [Display(Name = "Is Completed")]
        public bool isCompleted { get; set; }
        

        [Display(Name = "Assigment Date")]
        public DateTime AssigmentDate { get; set; }
    }
}
