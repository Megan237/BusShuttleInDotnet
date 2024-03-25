using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.Database;
namespace WebMvc.Service
{
    public class RouteService : RouteServiceInterface
    {
        private readonly BusDb _busDb;

        public RouteService(BusDb busDb)
        {
            _busDb = busDb;
        }
        public List<RouteList> GetRoutes()
        {
            var routeList = _busDb.Route.Select(r => new RouteList(r.Id, r.Order)).ToList();
            return routeList;
        }

        public void UpdateRouteByID(int id, int order)
        {
            var route = _busDb.Route.FirstOrDefault(r => r.Id == id);
            if (route != null)
            {
                route.Order = order;
                _busDb.SaveChanges();

            }

            public void CreateRoute(int order)
            {
                var newRoute = new Database.Route
                {
                    order = order
                };
                _busDb.Route.Add(newRoute);
                _busDb.SaveChanges();
            }
        }
    }





}