using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;
namespace WebMvc.Models
{
    public class DriverSignOnModel
    {
        public int BusId { get; set; }
        public int LoopId { get; set; }
    }
}
