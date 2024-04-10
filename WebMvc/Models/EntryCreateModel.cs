using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DomainModel;
namespace WebMvc.Models
{
    public class EntryCreateModel
    {
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }
        public int Boarded { get; set; }
        public int LeftBehind { get; set; }

        public int StopId { get; set; }
        public StopModel Stop { get; set; }
        public int LoopId { get; set; }
        public LoopModel Loop { get; set; }
        public int DriverId { get; set; }
        public DriverModel Driver { get; set; }
        public int BusId { get; set; }
        public BusModel Bus { get; set; }
    }
}