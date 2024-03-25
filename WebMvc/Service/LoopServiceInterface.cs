using DomainModel;
namespace WebMvc.Service;

public interface LoopServiceInterface
{
    List<LoopList> GetLoops();
    void UpdateLoopByID(int id, string name);
    void CreateLoop(string name);
}