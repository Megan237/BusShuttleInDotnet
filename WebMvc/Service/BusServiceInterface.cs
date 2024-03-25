using DomainModel;
namespace WebMvc.Service;

public interface BusServiceInterface
{
    List<BusList> GetBusses();
    void UpdateBusByID(int id, int busNumber);
    void CreateBus(int busNumber);
}