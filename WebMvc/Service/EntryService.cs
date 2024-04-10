using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebMvc.Database;
namespace WebMvc.Service
{
    public class EntryService : EntryServiceInterface
    {
        private readonly BusDb _busDb;

        public EntryService(BusDb busDb)
        {
            _busDb = busDb;
        }
        public List<EntryModel> GetEntries()
        {
            var entryList = _busDb.Entry.Select(e => new EntryModel(e.Id, e.TimeStamp, e.Boarded, e.LeftBehind, e.StopId, e.StopId, e.DriverId, e.LoopId)).ToList();
            return entryList;
        }

        public void UpdateEntryByID(int id, DateTime timeStamp, int boarded, int leftBehind)
        {
            var entry = _busDb.Entry.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                entry.TimeStamp = timeStamp;
                entry.Boarded = boarded;
                entry.LeftBehind = leftBehind;
                _busDb.SaveChanges();

            }
        }
        public void CreateEntry(DateTime timeStamp, int boarded, int leftBehind, int busId, int stopId, int driverId, int loopId)
        {
            var newEntry = new Database.Entry
            {
                TimeStamp = timeStamp,
                Boarded = boarded,
                LeftBehind = leftBehind,
                BusId = busId,
                LoopId = loopId,
                DriverId = driverId,
                StopId = stopId

            };
            _busDb.Entry.Add(newEntry);
            _busDb.SaveChanges();

        }

        public EntryModel? FindEntryByID(int id)
        {
            var entry = _busDb.Entry.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                return new EntryModel(entry.Id, entry.TimeStamp, entry.Boarded, entry.LeftBehind, entry.BusId, entry.StopId, entry.DriverId, entry.LoopId);
            }
            return null;
        }
        public void DeleteEntry(int id)
        {
            var entry = _busDb.Entry.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                _busDb.Entry.Remove(entry);
                _busDb.SaveChanges();
            }
        }

        public class EntryDetailDTO
        {
            public int Id { get; set; }
            public DateTime TimeStamp { get; set; }
            public int Boarded { get; set; }
            public int LeftBehind { get; set; }
            public string StopName { get; set; }
            public string LoopName { get; set; }
            public string DriverName { get; set; }
            public int BusNumber { get; set; }
        }

        public List<EntryDetailDTO> GetEntryDetails()
        {
            var entryDetails = _busDb.Entry
                .Include(r => r.Stop) // Ensure your Route entity has navigation properties to Stop and Loop
                .Include(r => r.Loop)
                .Include(r => r.Driver)
                .Include(r => r.Bus)
                .Select(r => new EntryDetailDTO
                {
                    Id = r.Id,
                    TimeStamp = r.TimeStamp,
                    Boarded = r.Boarded,
                    LeftBehind = r.LeftBehind,
                    StopName = r.Stop.Name, // Assuming Stop has a Name property
                    LoopName = r.Loop.Name, // Assuming Loop has a Name property
                    DriverName = r.Driver.FirstName + " " + r.Driver.LastName,
                    BusNumber = r.Bus.BusNumber
                }).ToList();

            return entryDetails;
        }
    }
}