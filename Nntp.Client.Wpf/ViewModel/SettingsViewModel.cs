using Nntp.Core.Models;
using Nntp.Core.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nntp.Client.Wpf.ViewModel;

/// <summary>
/// ViewModel that handles server settings for the GUI and 
/// coordinates saving/loading through ISettingsRepository.
/// </summary>
public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly ISettingsRepository _repository;

    public string ServerName { get; set; } = "";
    public string PortText { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string StatusMessage { get; private set; } = "";

    public SettingsViewModel() : this(new JsonSettingsRepository()) { }

    public SettingsViewModel(ISettingsRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Validates user input and saves the settings to JSON.
    /// </summary>
    public void Save()
    {
        // Validate server name
        if (string.IsNullOrWhiteSpace(ServerName))
        {
            StatusMessage = "Server is required.";
            OnPropertyChanged(nameof(StatusMessage));
            return;
        }

        // Validate port
        if (!int.TryParse(PortText, out int port))
        {
            StatusMessage = "Port must be a number.";
            OnPropertyChanged(nameof(StatusMessage));
            return;
        }

        // Create the settings object
        var settings = new ServerSettings
        {
            ServerName = ServerName.Trim(),
            Port = port,
            Username = Username,
            Password = Password
        };

        // Save via repository
        _repository.Save(settings);

        //Update status
        StatusMessage = "Settings saved.";
        OnPropertyChanged(nameof(StatusMessage));
    }

    /// <summary>
    /// Loads settings from JSON into the GUI fields.
    /// </summary>
    public void Load()
    {
        var settings = _repository.Load();
        if (settings is null)
        {
            StatusMessage = "No saved settings found.";
            OnPropertyChanged(nameof(StatusMessage));
            return;
        }

        ServerName = settings.ServerName;
        PortText = settings.Port.ToString();
        Username = settings.Username ?? "";
        Password = settings.Password ?? "";

        StatusMessage = "Settings loaded.";
        OnPropertyChanged(nameof(ServerName));
        OnPropertyChanged(nameof(PortText));
        OnPropertyChanged(nameof(Username));
        OnPropertyChanged(nameof(Password));
        OnPropertyChanged(nameof(StatusMessage));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
