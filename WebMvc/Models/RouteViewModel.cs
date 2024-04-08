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
        public StopModel Stop { get; set; }
        public int LoopId { get; set; }
        public LoopModel Loop { get; set; }

        public static RouteViewModel FromRoute(RouteModel route)
        {
            return new RouteViewModel
            {
                Id = route.Id,
                Order = route.Order,
                StopId = route.StopId,
                Stop = route.Stop,
                LoopId = route.LoopId,
                Loop = route.Loop,

            };
        }
    }
}