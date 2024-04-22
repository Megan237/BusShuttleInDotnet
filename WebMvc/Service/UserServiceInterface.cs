using DomainModel;
using WebMvc.Database;
namespace WebMvc.Service;

public interface UserServiceInterface
{
    List<UserModel> GetUsers();
    UserModel? FindUserByID(int id);
    void CreateUser(string firstname, string lastname, string username, string password);
    bool VerifyUserAsManager(string userName, string password);
    Driver VerifyUserAsDriver(string userName, string password);
    bool VerifyUser(string userName, string password);
}