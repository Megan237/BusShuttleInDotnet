using DomainModel;
namespace WebMvc.Service;

public interface RouteServiceInterface
{
    List<RouteModel> GetRoutes();
    void UpdateRouteByID(int id, int order);
    void CreateRoute(int order);
    RouteModel? FindRouteByID(int id);
}