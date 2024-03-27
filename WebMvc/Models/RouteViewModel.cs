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

        public static RouteViewModel FromRoute(RouteModel route)
        {
            return new RouteViewModel
            {
                Id = route.Id,
                Order = route.Order
            };
        }
    }
}