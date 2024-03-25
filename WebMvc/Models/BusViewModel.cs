using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;

namespace WebMvc.Models
{
    public class BusViewModel
    {
        public int Id { get; set; }
        public int BusNumber { get; set; }
        [Display(Name = "Bus Number")]

        public static BusViewModel FromBus(BusModel bus)
        {
            return new BusViewModel
            {
                Id = bus.Id,
                BusNumber = bus.BusNumber
            };
        }
    }
}