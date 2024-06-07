namespace AluraLibrary.Interfaces;

public interface IDataService
{
    public void WriteData(string path, string file, object data, bool refresh = false);
    public object? ReadData(string path, string file);
    public bool DataExists(string path, string file);
}
