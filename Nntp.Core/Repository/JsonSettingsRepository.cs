// Nntp.Core/Repository/JsonSettingsRepository.cs
using Newtonsoft.Json;
using Nntp.Core.Models;
using System;
using System.IO;
using Formatting = Newtonsoft.Json.Formatting;

namespace Nntp.Core.Repository;

/// <summary>
/// Persists ServerSettings to a JSON file (read/write).
/// Default ctor stores under %AppData%\NntpDemo\settings.json.
/// </summary>
public sealed class JsonSettingsRepository : ISettingsRepository
{
    private readonly string _path;

    //Default: AppData\NntpDemo\settings.json
    public JsonSettingsRepository()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NntpDemo", "settings.json"))
    {
    }

    // Overload: allow explicit path (for unit test use)
    public JsonSettingsRepository(string path)
    {
        _path = path;
    }

    public void Save(ServerSettings settings)
    {
        // Ensure target directory exists
        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(_path, json);

        //System.Diagnostics.Debug.WriteLine($"Saving settings to: {_path}");

    }

    public ServerSettings? Load()
    {
        if (!File.Exists(_path)) return null;

        var json = File.ReadAllText(_path);
        return JsonConvert.DeserializeObject<ServerSettings>(json);
    }
}
