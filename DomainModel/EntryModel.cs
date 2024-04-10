namespace DomainModel;

public class EntryModel
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    public int StopId { get; set; }
    public StopModel Stop { get; set; }
    public int LoopId { get; set; }
    public LoopModel Loop { get; set; }
    public int DriverId { get; set; }
    public DriverModel Driver { get; set; }
    public int BusId { get; set; }
    public BusModel Bus { get; set; }


    public EntryModel(int id, DateTime timestamp, int boarded, int leftbehind, int stopId, int loopId, int driverId, int busId)
    {
        Id = id;
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
        StopId = stopId;
        LoopId = loopId;
        DriverId = driverId;
        BusId = busId;
    }

    public void Update(DateTime timestamp, int boarded, int leftbehind)
    {
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
    }
}