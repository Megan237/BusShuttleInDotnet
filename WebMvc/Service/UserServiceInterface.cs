using DomainModel;
namespace WebMvc.Service;

public interface UserServiceInterface
{
    List<UserModel> GetUsers();
    UserModel? FindUserByID(int id);
    void CreateUser(string firstname, string lastname, string username, string password);
    bool VerifyUserAsManager(string userName, string password);
    bool VerifyUserAsDriver(string userName, string password);
}