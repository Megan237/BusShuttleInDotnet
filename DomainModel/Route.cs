namespace DomainModel;

public class Route
{
    public int Id { get; set; }
    public int Order { get; set; }

    public Route(int id, int order)
    {
        Id = id;
        Order = order;
    }

    public void Update(int order)
    {
        Order = order;
    }
}