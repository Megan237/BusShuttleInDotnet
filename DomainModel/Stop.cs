namespace DomainModel;

public class Stop
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Stop(int id, string name, double latitude, double longitude)
    {
        Id = id;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void Update(string name, double latitude, double longitude)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
    }
}