using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.Database;
namespace WebMvc.Service
{
    public class LoopService : LoopServiceInterface
    {
        private readonly BusDb _busDb;

        public LoopService(BusDb busDb)
        {
            _busDb = busDb;
        }
        public List<LoopModel> GetLoops()
        {
            var loopList = _busDb.Loop.Select(l => new LoopModel(l.Id, l.Name)).ToList();
            return loopList;
        }

        public void UpdateLoopByID(int id, string name)
        {
            var loop = _busDb.Loop.FirstOrDefault(l => l.Id == id);
            if (loop != null)
            {
                loop.Name = name;
                _busDb.SaveChanges();

            }
        }
        public void CreateLoop(string name)
        {
            var newLoop = new Database.Loop
            {
                Name = name
            };
            _busDb.Loop.Add(newLoop);
            _busDb.SaveChanges();
        }
    }
}