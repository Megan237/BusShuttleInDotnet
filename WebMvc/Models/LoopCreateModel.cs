using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DomainModel;
namespace WebMvc.Models
{
    public class LoopCreateModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}