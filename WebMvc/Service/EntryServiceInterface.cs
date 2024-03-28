using DomainModel;
namespace WebMvc.Service;

public interface EntryServiceInterface
{
    List<EntryModel> GetEntries();
    void UpdateEntryByID(int id, DateTime timeStamp, int boarded, int leftBehind);
    void CreateEntry(DateTime timeStamp, int boarded, int leftBehind);
    EntryModel? FindEntryByID(int id);
}