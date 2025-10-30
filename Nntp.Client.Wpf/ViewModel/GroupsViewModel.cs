using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Nntp.Core.Models;

namespace Nntp.Client.Wpf.ViewModel
{
    /// <summary>
    /// ViewModel that loads and exposes newsgroups to the GUI.
    /// </summary>
    public sealed class GroupsViewModel : INotifyPropertyChanged
    {
        private readonly INntpClientApi _client;

        public GroupsViewModel(INntpClientApi client)
        {
            _client = client;
        }

        /// <summary>
        /// Bindable collection of groups shown in the ListBox.
        /// </summary>
        public ObservableCollection<NewsGroup> Groups { get; } = new();

        private string _status = "Ready";
        /// <summary>
        /// Bindable status line (bottom of the window).
        /// </summary>
        public string StatusMessage
        {
            get => _status;
            private set { _status = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Loads groups via the client API and updates the observable collection.
        /// </summary>
        public async Task LoadGroupsAsync()
        {
            StatusMessage = "Loading groups...";

            var data = await _client.ListGroupsAsync();

            // Refresh the collection so the UI updates automatically
            Groups.Clear();
            foreach (var g in data)
                Groups.Add(g);

            StatusMessage = $"Loaded {Groups.Count} groups.";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
