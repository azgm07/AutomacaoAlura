using AluraLibrary.Interfaces;
using AluraLibrary.Models;
using Newtonsoft.Json;

namespace AluraLibrary.Services;

internal class DataService : IDataService
{
    public bool DataExists(string path, string file)
    {
        string filePath = Path.Combine(path, file);
        string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string sFile = Path.Combine(sCurrentDirectory, filePath);
        string sFilePath = Path.GetFullPath(sFile);

        if (File.Exists(sFilePath))
        {
            return true;
        }

        return false;
    }

    public object? ReadData(string path, string file)
    {
        string filePath = Path.Combine(path, file);
        string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string sPath = Path.Combine(sCurrentDirectory, path);
        string sFile = Path.Combine(sCurrentDirectory, filePath);
        string sFullPath = Path.GetFullPath(sPath);
        string sFilePath = Path.GetFullPath(sFile);

        if (!Directory.Exists(sFullPath))
        {
            Directory.CreateDirectory(sFullPath);
        }

        CourseInformation? data = new();

        if (File.Exists(sFilePath))
        {
            using var reader = new StreamReader(sFilePath);
            JsonSerializer serializer = new();
            data = (CourseInformation?)serializer.Deserialize(reader, typeof(CourseInformation));
        }
        return data;
    }

    public void WriteData(string path, string file, object data, bool refresh = false)
    {
        string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string sPath = Path.Combine(sCurrentDirectory, path);
        string sFile = Path.Combine(sPath, file);
        string sFullPath = Path.GetFullPath(sPath);
        string sFilePath = Path.GetFullPath(sFile);

        if (!Directory.Exists(sFullPath))
        {
            Directory.CreateDirectory(sFullPath);
        }

        if (refresh && File.Exists(sFilePath))
        {
            File.Delete(sFilePath);
        }

        using FileStream fs = new(sFilePath, FileMode.Append, FileAccess.Write);
        using StreamWriter sw = new(fs);

        JsonSerializer serializer = new();
        serializer.Serialize(sw, data);
    }
}
