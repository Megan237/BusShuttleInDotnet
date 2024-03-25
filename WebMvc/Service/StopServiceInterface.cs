using DomainModel;
namespace WebMvc.Service;

public interface StopServiceInterface
{
    List<StopList> GetStops();
    void UpdateStopByID(int id, string name, double latitude, double longitude);
    void CreateStop(string name, double latitude, double longitude);
}