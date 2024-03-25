using DomainModel;
namespace WebMvc.Service;

public interface RouteServiceInterface
{
    List<RouteList> GetRoutes();
    void UpdateRouteByID(int id, int order);
    void CreateRoute(int order);
}