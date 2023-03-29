namespace Domain.Network;

public class Statistic
{
    public string Name;
    public string FileName;

    public Statistic(string name,string fileName)
    {
        FileName = fileName;
        Name = name;
    }
}