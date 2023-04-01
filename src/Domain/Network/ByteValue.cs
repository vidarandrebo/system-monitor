namespace Domain.Network;

public class ByteValue : IValue
{
    private long _current;
    private long _max = Int64.MinValue;
    private long _min = Int64.MaxValue;
    
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

    private string BinaryPrefixedValue(long value)
    {
        if (value > BinaryPrefix.Tebi)
        {
            return $"{(Convert.ToDouble(value) / BinaryPrefix.Tebi):0.00} TiB";
        }
        if (value > BinaryPrefix.Gibi)
        {
            return $"{(Convert.ToDouble(value) / BinaryPrefix.Gibi):0.00} GiB";
        }
        if (value > BinaryPrefix.Mebi)
        {
            return $"{(Convert.ToDouble(value) / BinaryPrefix.Mebi):0.00} MiB";
        }
        if (value > BinaryPrefix.Kibi)
        {
            return $"{(Convert.ToDouble(value) / BinaryPrefix.Kibi):0.00} kiB";
        }

        return value.ToString() + " B";
    }

    public ValueDTO GetRecord()
    {
        return new ValueDTO(BinaryPrefixedValue(_current), BinaryPrefixedValue(_min), BinaryPrefixedValue(_max), "");
    }
}