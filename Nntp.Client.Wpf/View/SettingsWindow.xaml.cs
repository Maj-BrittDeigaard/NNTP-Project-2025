using System.Windows;
using Nntp.Client.Wpf.ViewModel;

namespace Nntp.Client.Wpf.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml.
    /// Keeps PasswordBox and ViewModel in sync.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsWindow(SettingsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Auto-load existing settings when window opens
            _viewModel.Load();
        }

        // When window loads, sync ViewModel.Password -> PasswordBox
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = _viewModel.Password ?? string.Empty;
        }

        // When user types in PasswordBox, sync PasswordBox -> ViewModel
        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        // Load existing settings from repository and update PasswordBox
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
            PasswordBox.Password = _viewModel.Password ?? string.Empty;
        }

        // Save changes and include password
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
            _viewModel.Save();
        }

        // Close window
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
