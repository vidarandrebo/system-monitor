namespace Domain.Temperature;

public class TemperatureValue : IValue
{
    private double _current;
    private double _max = double.MinValue;
    private double _min = double.MaxValue;

    public void Update(string newValue)
    {
        try
        {
            _current = double.Parse(newValue.Trim()) / 1000.0;
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException)
        {
            _current = 0;
        }

        MinMax();
    }

    private void MinMax()
    {
        if (_current > _max)
        {
            _max = _current;
        }

        if (_current < _min)
        {
            _min = _current;
        }
    }

    public ValueDTO GetRecord()
    {
        return new ValueDTO($"{_current:0.00}", $"{_min:0.00}", $"{_max:0.00}", "");
    }
}