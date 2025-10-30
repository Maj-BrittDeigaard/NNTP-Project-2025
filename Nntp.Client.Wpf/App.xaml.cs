using System;
using System.IO;
using System.Windows;
using Nntp.Core.Repository;

namespace Nntp.Client.Wpf
{
    /// <summary>
    /// Creates a single repository instance pointing to %AppData%\NntpDemo\settings.json
    /// </summary>
    public partial class App : Application
    {
        public static ISettingsRepository SettingsRepo { get; } =
            new JsonSettingsRepository(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                             "NntpDemo", "settings.json"));
    }
}

