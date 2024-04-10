using DomainModel;
namespace WebMvc.Service;

public interface EntryServiceInterface
{
    List<EntryModel> GetEntries();
    void UpdateEntryByID(int id, DateTime timeStamp, int boarded, int leftBehind);
    void CreateEntry(DateTime timeStamp, int boarded, int leftBehind, int busId, int stopId, int driverId, int loopId);
    EntryModel? FindEntryByID(int id);
    void DeleteEntry(int id);
    List<EntryService.EntryDetailDTO> GetEntryDetails();
}