namespace Domain.Network;

public class Statistic
{
    public string Name;
    public string FileName;
    public IValue Value;

    public Statistic(string name,string fileName)
    {
        FileName = fileName;
        Name = name;
        Value = new IntegerValue();
    }

    public void Update(string newValue)
    {
        Value.Update(newValue);
    }
}