namespace Domain;

public class IntegerValue : IValue
{
    private long _current;
    private long _max;
    private long _min;
    
    public void Update(string newValue)
    {
        try
        {
            _current = long.Parse(newValue.Trim());
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
        return new ValueDTO(_current.ToString(), _min.ToString(), _max.ToString(), "");
    }
}