using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;

namespace WebMvc.Models
{
    public class DriverScreenModel
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Boarded { get; set; }
        public int LeftBehind { get; set; }
        public int BusId { get; set; }
        public int LoopId { get; set; }
        public int StopId { get; set; }


        public static BusViewModel FromDriverScreen(BusModel bus)
        {
            return new BusViewModel
            {
                Id = bus.Id,
                BusNumber = bus.BusNumber,
            };
        }
    }
}
