using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WukkaManCleaning_API.Models
{
    public class EmplyeeClockShift
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    


        [Column(TypeName = "varchar(50)")]
        public int WorkId { get; set; }


        public DateTime ClockIn { get; set; }
        public DateTime ClockOut { get; set; }
        public DateTime ShiftDate { get; set; }
    }
}
