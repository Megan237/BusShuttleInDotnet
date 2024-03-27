using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DomainModel;
namespace WebMvc.Models
{
    public class LoopEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public static LoopEditModel FromLoop(LoopModel loop)
        {
            return new LoopEditModel
            {
                Id = loop.Id,
                Name = loop.Name
            };
        }
    }
}