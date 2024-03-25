namespace DomainModel;

public class BusModel
{
    public int Id { get; set; }
    public int BusNumber { get; set; }

    public BusModel(int id, int busNumber)
    {
        Id = id;
        BusNumber = busNumber;
    }

    public void Update(int busNumber)
    {
        BusNumber = busNumber;
    }
}
