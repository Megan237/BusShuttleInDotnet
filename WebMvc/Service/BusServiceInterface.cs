using DomainModel;
namespace WebMvc.Service;

public interface BusServiceInterface
{
    List<BusModel> GetBusses();
    void UpdateBusByID(int id, int busNumber);
    void CreateBus(int busNumber);

    BusModel? FindBusByID(int id);
    void DeleteBus(int id);
}