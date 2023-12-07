using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace WukkaMan_Interface.Models
{
    public class EmplyeeClockShift
    {
        public int Id { get; set; }


        //fk
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }




        [Display(Name = "Work ID")]
        public int WorkId { get; set; }


        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        [Display(Name = "Clocked In")]
        public DateTime ClockIn { get; set; }


        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        [Display(Name = "Clocked Out")]
        public DateTime ClockOut { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Shift Date")]
        public DateTime ShiftDate { get; set; }

    }
}
