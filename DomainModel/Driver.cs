namespace DomainModel;

public class Driver
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Driver(int id, string firstname, string lastname)
    {
        Id = id;
        FirstName = firstname;
        LastName = lastname;
    }

    public void Update(string firstname, string lastname)
    {
        FirstName = firstname;
        LastName = lastname;
    }
}