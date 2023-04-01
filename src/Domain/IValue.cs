namespace Domain;

public interface IValue
{
    void Update(string newValue);
    ValueDTO GetRecord();
}