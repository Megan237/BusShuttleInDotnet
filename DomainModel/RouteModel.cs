namespace DomainModel;

public class RouteModel
{
    public int Id { get; set; }
    public int Order { get; set; }

    public RouteModel(int id, int order)
    {
        Id = id;
        Order = order;
    }

    public void Update(int order)
    {
        Order = order;
    }
}