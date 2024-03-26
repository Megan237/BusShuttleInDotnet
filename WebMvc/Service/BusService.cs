using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.Database;
namespace WebMvc.Service
{
    public class BusService : BusServiceInterface
    {
        private readonly BusDb _busDb;

        public BusService(BusDb busDb)
        {
            _busDb = busDb;
        }
        public List<BusModel> GetBusses()
        {
            var busList = _busDb.Bus.Select(b => new BusModel(b.Id, b.BusNumber)).ToList();
            return busList;
        }

        public void UpdateBusByID(int id, int busNumber)
        {
            var bus = _busDb.Bus.FirstOrDefault(b => b.Id == id);
            if (bus != null)
            {
                bus.BusNumber = busNumber;
                _busDb.SaveChanges();

            }
        }

        public void CreateBus(int busNumber)
        {
            var newBus = new Database.Bus
            {
                BusNumber = busNumber
            };
            _busDb.Bus.Add(newBus);
            _busDb.SaveChanges();

        }
    }





}