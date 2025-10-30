using Nntp.Client.Wpf.ViewModel;
using Nntp.Core.Models;
using Nntp.Core.Repository;
using Xunit;

namespace Nntp.Tests.ViewModel
{
    // Simple in-memory fake of the repository
    internal sealed class FakeSettingsRepository : ISettingsRepository
    {
        public ServerSettings? Saved { get; private set; }
        public ServerSettings? Load() => null;
        public void Save(ServerSettings settings) => Saved = settings;
    }

    public sealed class SettingsViewModelTests
    {
        [Fact]
        public void Save_ValidInput_PersistsToRepository_AndSetsOkStatus()
        {
            // Arrange
            var repo = new FakeSettingsRepository();
            var vm = new SettingsViewModel(repo)
            {
                ServerName = "news.sunsite.dk",
                PortText = "119",
                Username = "testuser",
                Password = "secret"
            };

            // Act
            vm.Save();

            // Assert
            Assert.NotNull(repo.Saved);
            Assert.Equal("news.sunsite.dk", repo.Saved!.ServerName);
            Assert.Equal(119, repo.Saved.Port);
            Assert.Equal("testuser", repo.Saved.Username);
            Assert.Equal("secret", repo.Saved.Password);
            Assert.Equal("Settings saved.", vm.StatusMessage);
        }

        [Fact]
        public void Save_MissingServer_SetsErrorStatus_AndDoesNotCallRepo()
        {
            // Arrange
            var repo = new FakeSettingsRepository();
            var vm = new SettingsViewModel(repo)
            {
                ServerName = "",   // invalid
                PortText = "119"
            };

            // Act
            vm.Save();

            // Assert
            Assert.Null(repo.Saved);
            Assert.Equal("Server is required.", vm.StatusMessage);
        }
    }
}


