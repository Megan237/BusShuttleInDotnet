using DomainModel;
namespace WebMvc.Service;

public interface DriverServiceInterface
{
    List<DriverList> GetDrivers();
    void UpdateDriverByID(int id, string firstname, string lastname);
    void CreateDriver(string firstname, string lastname);
}