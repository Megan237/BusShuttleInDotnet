namespace DomainModel;

public class UserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserModel()
    {
    }
    public UserModel(int id, string firstname, string lastname, string username, string password)
    {
        Id = id;
        FirstName = firstname;
        LastName = lastname;
        UserName = username;
        Password = password;
    }

    public void Update(string firstname, string lastname, string username, string password)
    {
        FirstName = firstname;
        LastName = lastname;
        UserName = username;
        Password = password;
    }
}