using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WukkaManCleaning_API.Models
{
    public class EmployeeTask
    {
        [Key]
        public int Id { get; set; }

      
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }



        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual EmpTask? Task { get; set; }


        public bool isCompleted { get; set; }

        public DateTime AssigmentDate { get; set; }
    }
}
