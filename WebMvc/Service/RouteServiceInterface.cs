using DomainModel;
namespace WebMvc.Service;

public interface RouteServiceInterface
{
    List<RouteModel> GetRoutes();
    void UpdateRouteByID(int id, int order, int stopId, StopModel stop, int loopId, LoopModel loop);
    void CreateRoute(int order, int stopId, StopModel stop, int loopId, LoopModel loop);
    RouteModel? FindRouteByID(int id);
    void DeleteRoute(int id);
    void SwapOrders(int currentId, int updatedId);
}