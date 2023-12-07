using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace WukkaMan_Interface.Models
{
    public class EmpTask
    {
        public int Id { get; set; }


        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
    }
}
