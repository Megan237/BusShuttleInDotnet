using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DomainModel;
namespace WebMvc.Models
{
    public class RouteCreateModel
    {
        public int Id { get; set; }

        public int Order { get; set; }
    }
}