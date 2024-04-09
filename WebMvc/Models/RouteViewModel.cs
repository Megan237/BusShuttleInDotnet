using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using DomainModel;

namespace WebMvc.Models
{
    public class RouteViewModel
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int StopId { get; set; }
        public string StopName { get; set; } // Added StopName
        public int LoopId { get; set; }
        public string LoopName { get; set; } // Added LoopName

        public static RouteViewModel FromRoute(RouteModel route)
        {
            return new RouteViewModel
            {
                Id = route.Id,
                Order = route.Order,
                StopId = route.StopId,
                StopName = route.Stop.Name, // Assuming route.Stop is not null
                LoopId = route.LoopId,
                LoopName = route.Loop.Name, // Assuming route.Loop is not null
            };
        }
    }
}
