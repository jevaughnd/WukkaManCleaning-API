using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WukkaMan_Interface.Models.ViewModel
{
    public class ClockShiftVM
    {
        public int Id { get; set; }

        //fk
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
    
        //values
        public List<SelectListItem>? EmployeeList { get; set; }

        //Selected DDL Value
        public int SelectedEployeeId { get; set; }


        [Display(Name = "Work ID")]
        public int WorkId { get; set; }

        [Display(Name = "Clocked In")]
        public DateTime ClockIn { get; set; }

        [Display(Name = "Clocked Out")]
        public DateTime ClockOut { get; set; }


        [Display(Name = "Shift Date")]
        public DateTime ShiftDate { get; set; }
    }
}
  