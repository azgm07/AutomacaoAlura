using AluraLibrary.Interfaces;
using AluraLibrary.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AluraLibrary.Services;

internal class DataService : IDataService
{
    private readonly ILogger<DataService> _logger;

    public DataService(ILogger<DataService> logger)
    {
        _logger = logger;
    }

    public bool DataExists(string path, string file)
    {
        string filePath = Path.Combine(path, file);
        string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string sFile = Path.Combine(sCurrentDirectory, filePath);
        string sFilePath = Path.GetFullPath(sFile);

        if (File.Exists(sFilePath))
        {
            _logger.LogDebug("DataService -> DataExists: Arquivo \"{file}\" encontrado.", sFilePath);
            return true;
        }
        _logger.LogDebug("DataService -> DataExists: Arquivo \"{file}\" não encontrado.", sFilePath);
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
            _logger.LogDebug("DataService -> ReadData: Arquivo \"{file}\" lido com sucesso.", sFilePath);
        }
        else
        {
            _logger.LogDebug("DataService -> ReadData: Arquivo \"{file}\" não foi lido.", sFilePath);
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
            _logger.LogDebug("DataService -> WriteData: Arquivo \"{file}\" deletado.", sFilePath);
        }

        using FileStream fs = new(sFilePath, FileMode.Append, FileAccess.Write);
        using StreamWriter sw = new(fs);

        JsonSerializer serializer = new();
        serializer.Serialize(sw, data);
        var beautifulJson = JsonConvert.SerializeObject(data, Formatting.Indented);
        _logger.LogDebug("DataService -> WriteData: Arquivo \"{file}\" gravado com sucesso.", sFilePath);
        _logger.LogDebug("DataService -> WriteData: Dados gravados: \n{json}", beautifulJson);
    }
}
