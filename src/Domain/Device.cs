using Domain.Network;
using Domain.Temperature;

namespace Domain;

public class Device
{
    public string Name;
    public readonly string FileName;
    public IValue Value;

    public Device(string name, string fileName)
    {
        FileName = fileName;
        Name = name;
        Value = new IntegerValue();
        if (name.Contains("bytes"))
        {
            Value = new ByteValue();
        }
        else if (name.Contains("temp"))
        {
            Value = new TemperatureValue();
        }
    }

    public void Update(string newValue)
    {
        Value.Update(newValue);
    }
}