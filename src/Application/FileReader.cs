using Domain;

namespace Application;

public static class FileReader
{
    public static Result<string> ReadFileToString(string fileName)
    {
        try
        {
            using var sr = new StreamReader(fileName);
            return new Result<string>(sr.ReadToEnd().Trim(), null);
        }
        catch (Exception ex) when (ex is IOException or OutOfMemoryException)
        {
            return new Result<string>("", new[] { ex.ToString() });
        }
    }
}