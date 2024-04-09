using DomainModel;
namespace WebMvc.Service;

public interface RouteServiceInterface
{
    List<RouteModel> GetRoutes();
    void UpdateRouteByID(int id, int order, int stopId, int loopId);
    void CreateRoute(int order, int stopId, int loopId);
    RouteModel? FindRouteByID(int id);
    void DeleteRoute(int id);
    void SwapOrders(int currentId, int updatedId);
    List<RouteService.RouteDetailDTO> GetRouteDetails();
}