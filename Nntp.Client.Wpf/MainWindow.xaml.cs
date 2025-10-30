using System.Windows;
using System.Windows.Controls;
using Nntp.Client.Wpf.View;
using Nntp.Client.Wpf.ViewModel;
using Nntp.Core.Models;

namespace Nntp.Client.Wpf
{
    /// <summary>
    /// Mainwindow: Interaction logic for MainWindow.xaml
    /// Opens the settings window from a menu command.
    /// Also wires GroupsViewModel and loads groups on startup.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GroupsViewModel _vm;
        private readonly HeadlinesViewModel _headsVm;

        public MainWindow()
        {
            InitializeComponent();

            // Use the real adapter which reads App.SettingsRepo and connects over TCP
            var clientApi = new NntpClientApiAdapter(App.SettingsRepo);
            _vm = new GroupsViewModel(clientApi);
            _headsVm = new HeadlinesViewModel(clientApi);

            DataContext = new
            {
                GroupsVM = _vm,
                HeadlinesVM = _headsVm
            };

            Loaded += async (_, __) =>
            {
                try {await _vm.LoadGroupsAsync();}
                catch (Exception ex) {_vm.GetType().GetProperty("StatusMessage")!
                      .SetValue(_vm, $"Error loading groups: {ex.Message}");}
            };
        }

        // Called when a group is selected in the left ListBox
        private async void GroupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupsList.SelectedItem is NewsGroup g)
                await _headsVm.LoadArticlesAsync(g.Name);
        }

        //Called when headline is clicked 
        private async void HeadlinesList_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            if (HeadlinesList.SelectedItem is ArticleHead a)
                await _headsVm.LoadArticleAsync(a.Number);
        }
        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var vm = new SettingsViewModel(App.SettingsRepo);
            new SettingsWindow(vm) { Owner = this }.ShowDialog();
            StatusText.Text = "Settings updated.";
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();
    }
}
