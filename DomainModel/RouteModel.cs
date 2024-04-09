namespace DomainModel;

public class RouteModel
{
    public int Id { get; set; }
    public int Order { get; set; }
    public int StopId { get; set; }
    public StopModel Stop { get; set; }
    public int LoopId { get; set; }
    public LoopModel Loop { get; set; }

    public RouteModel(int id, int order, int stopId, int loopId)
    {
        Id = id;
        Order = order;
        StopId = stopId;
        LoopId = loopId;
    }

    public void Update(int order, int stopId, StopModel stop, int loopId, LoopModel loop)
    {
        Order = order;
        StopId = stopId;
        LoopId = loopId;
    }
}