using DomainModel;
namespace WebMvc.Service;

public interface EntryServiceInterface
{
    List<EntryList> GetEntries();
    void UpdateEntryByID(int id, DateTime timeStamp, int boarded, int leftBehind);
    void CreateEntry(DateTime timeStamp, int boarded, int leftBehind);
}