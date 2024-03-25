using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.Database;
namespace WebMvc.Service
{
    public class DriverService : DriverServiceInterface
    {
        private readonly BusDb _busDb;

        public DriverService(BusDb busDb)
        {
            _busDb = busDb;
        }
        public List<DriverList> GetDrivers()
        {
            var driverList = _busDb.Driver.Select(d => new DriverList(d.Id, d.FirstName, d.LastName)).ToList();
            return driverList;
        }

        public void UpdateDriverByID(int id, string firstname, string lastname)
        {
            var driver = _busDb.Driver.FirstOrDefault(d => d.Id == id);
            if (driver != null)
            {
                driver.FirstName = firstname;
                driver.LastName = lastname;
                _busDb.SaveChanges();

            }

            public void CreateDriver(string firstname, string lastname)
            {
                var newDriver = new Database.Driver
                {
                    FirstName = firstname,
                    lastname = lastname
                };
                _busDb.Driver.Add(newDriver);
                _busDb.SaveChanges();
            }
        }
    }





}