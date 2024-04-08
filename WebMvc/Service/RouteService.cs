using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using Microsoft.VisualBasic;
using WebMvc.Database;
using DomainModel;
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
            var routeList = _busDb.Route.Select(r => new RouteModel(r.Id, r.Order, route.StopId, route.Stop, route.LoopId, route.Loop)).ToList();
            return routeList;
        }

        public void UpdateRouteByID(int id, int order, int stopId, StopModel stop, int loopId, LoopModel loop)
        {
            var route = _busDb.Route.FirstOrDefault(r => r.Id == id);
            if (route != null)
            {
                route.Order = order;
                route.StopId = stopId;
                route.Stop = stop;
                route.LoopId = loopId;
                route.Loop = loop;
                _busDb.SaveChanges();

            }
        }
        public void CreateRoute(int order, int stopId, StopModel stop, int loopId, LoopModel loop)
        {
            var newRoute = new Database.Route
            {
                Order = order,
                StopId = stopId,
                Stop = stop,
                LoopId = loopId,
                Loop = loop,

            };
            _busDb.Route.Add(newRoute);
            _busDb.SaveChanges();
        }

        public RouteModel? FindRouteByID(int id)
        {
            var route = _busDb.Route.FirstOrDefault(r => r.Id == id);
            if (route != null)
            {
                return new RouteModel(route.Id, route.Order, route.StopId, route.Stop, route.LoopId, route.Loop);
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
    }
}