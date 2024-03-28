using DomainModel;
namespace WebMvc.Service;

public interface DriverServiceInterface
{
    List<DriverModel> GetDrivers();
    void UpdateDriverByID(int id, string firstname, string lastname);
    void CreateDriver(string firstname, string lastname);
    DriverModel? FindDriverByID(int id);
    void DeleteDriver(int id);
}