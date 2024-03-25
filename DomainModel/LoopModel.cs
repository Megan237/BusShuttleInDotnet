namespace DomainModel;

public class LoopModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public LoopModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}