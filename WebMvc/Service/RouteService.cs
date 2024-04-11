using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.Database;
using DomainModel;
using Microsoft.EntityFrameworkCore;
namespace WebMvc.Service
{
    public class RouteService : RouteServiceInterface
    {
        private readonly BusDb _busDb;

        public RouteService(BusDb busDb)
        {
            _busDb = busDb;
        }
        public List<RouteModel> GetRoutes()
        {
            var routeList = _busDb.Route.Select(r => new RouteModel(r.Id, r.Order, r.StopId, r.LoopId)).ToList();
            return routeList;
        }

        public void UpdateRouteByID(int id, int order, int stopId, int loopId)
        {
            var route = _busDb.Route.FirstOrDefault(r => r.Id == id);
            if (route != null)
            {
                route.Order = order;
                route.StopId = stopId;
                route.LoopId = loopId;
                _busDb.SaveChanges();

            }
        }
        public void CreateRoute(int order, int stopId, int loopId)
        {
            var newRoute = new Database.Route
            {
                Order = order,
                StopId = stopId,
                LoopId = loopId,

            };
            _busDb.Route.Add(newRoute);
            _busDb.SaveChanges();
        }

        public RouteModel? FindRouteByID(int id)
        {
            var route = _busDb.Route.FirstOrDefault(r => r.Id == id);
            if (route != null)
            {
                return new RouteModel(route.Id, route.Order, route.StopId, route.LoopId);
            }
            return null;
        }
        public void DeleteRoute(int id)
        {
            var route = _busDb.Route.FirstOrDefault(r => r.Id == id);
            if (route != null)
            {
                _busDb.Route.Remove(route);
                _busDb.SaveChanges();
            }
        }

        public void SwapOrders(int currentId, int updatedId)
        {
            var currentRoute = _busDb.Route.FirstOrDefault(r => r.Id == currentId);
            var updatedRoute = _busDb.Route.FirstOrDefault(r => r.Id == updatedId);

            if (currentRoute != null && updatedRoute != null)
            {
                var currentOrder = currentRoute.Order;
                var updatedOrder = updatedRoute.Order;

                currentRoute.Order = updatedOrder;
                updatedRoute.Order = currentOrder;
                _busDb.SaveChangesAsync();
            }
        }

        public class RouteDetailDTO
        {
            public int Id { get; set; }
            public int Order { get; set; }
            public string StopName { get; set; }
            public string LoopName { get; set; }

            // Add other properties as needed
        }
        public class RouteDetailsWithLoopDTO
        {
            public int Id { get; set; }
            public int Order { get; set; }
            public string StopName { get; set; }
            public int StopId { get; set; }

            // Add other properties as needed
        }

        public List<RouteDetailDTO> GetRouteDetails()
        {
            var routeDetails = _busDb.Route
                .Include(r => r.Stop) // Ensure your Route entity has navigation properties to Stop and Loop
                .Include(r => r.Loop)
                .Select(r => new RouteDetailDTO
                {
                    Id = r.Id,
                    Order = r.Order,
                    StopName = r.Stop.Name, // Assuming Stop has a Name property
                    LoopName = r.Loop.Name // Assuming Loop has a Name property
                }).ToList();

            return routeDetails;
        }

        public List<RouteDetailsWithLoopDTO> GetRouteDetailsByLoop(int loopId)
        {
            var routeDetails = _busDb.Route
                .Include(r => r.Stop) // Ensure your Route entity has navigation properties to Stop and Loop
                .Include(r => r.Loop)
                .Where(r => r.Loop.Id == loopId) // Filter based on loopId
                .Select(r => new RouteDetailsWithLoopDTO
                {
                    Id = r.Id,
                    Order = r.Order,
                    StopName = r.Stop.Name, // Assuming Stop has a Name property
                    StopId = r.Stop.Id // Assuming Loop has a Name property
                }).ToList();
            return routeDetails;
        }
    }
}