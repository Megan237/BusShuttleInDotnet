using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;

namespace WebMvc.Models
{
    public class EntryViewModel
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Boarded { get; set; }
        public int LeftBehind { get; set; }
        public int LoopId { get; set; }
        public string LoopName { get; set; }
        public int StopId { get; set; }
        public string StopName { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int BusId { get; set; }
        public int BusNumber { get; set; }

        public static EntryViewModel FromEntry(EntryModel entry)
        {
            return new EntryViewModel
            {
                Id = entry.Id,
                TimeStamp = entry.TimeStamp,
                Boarded = entry.Boarded,
                LeftBehind = entry.LeftBehind,
                LoopId = entry.LoopId,
                LoopName = entry.Loop.Name,
                StopId = entry.StopId,
                StopName = entry.Stop.Name,
                DriverId = entry.DriverId,
                DriverName = entry.Driver.FirstName + " " + entry.Driver.LastName,
                BusId = entry.BusId,
                BusNumber = entry.Bus.BusNumber

            };
        }
    }
}