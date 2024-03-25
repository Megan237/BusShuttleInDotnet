namespace DomainModel;

public class Entry
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }

    public Entry(int id, DateTime timestamp, int boarded, int leftbehind)
    {
        Id = id;
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
    }

    public void Update(DateTime timestamp, int boarded, int leftbehind)
    {
        TimeStamp = timestamp;
        Boarded = boarded;
        LeftBehind = leftbehind;
    }
}