using System;

namespace Domain;

public class DeviceTemperature
{
    public readonly Guid Id;
    public double Value;
    public double Max;
    public double Min;
    public readonly string FileName;

    public DeviceTemperature(string fileName)
    {
        Max = Int64.MinValue;
        Min = Int64.MaxValue;
        Value = 0.0;
        Id = Guid.NewGuid();
        FileName = fileName;
    }

    public void SetTemp(Int64 value)
    {
        try
        {
            Value = Convert.ToDouble(value) / 1000;
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            Value = -1.0;
            return;
        }

        if (Value > Max)
        {
            Max = Value;
        }

        if (Value < Min)
        {
            Min = Value;
        }
    }
}