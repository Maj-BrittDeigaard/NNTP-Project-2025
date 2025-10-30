using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Nntp.Core.Models;
using Nntp.Client.Wpf.ViewModel;

// Simple fake NntpClient for ViewModel tests
file sealed class FakeNntpClient : INntpClientApi
{
    public bool WasCalled { get; private set; }

    public Task<IReadOnlyList<NewsGroup>> ListGroupsAsync()
    {
        WasCalled = true;
        IReadOnlyList<NewsGroup> data = new[]
        {
            new NewsGroup { Name = "comp.lang.c", HighWatermark = 123, LowWatermark = 1, PostingFlag = 'y' },
            new NewsGroup { Name = "dk.edb.programmering", HighWatermark = 456, LowWatermark = 10, PostingFlag = 'n' }
        };
        return Task.FromResult(data);
    }
    public Task<string> GetArticleAsync(int articleNumber)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ArticleHead>> ListArticlesAsync(string groupName)
    {
        throw new NotImplementedException();
    }
}

public class GroupsViewModelTests
{
    [Fact]
    public async Task LoadGroups_PopulatesCollection_AndSetsStatus()
    {
        // Arrange
        var fake = new FakeNntpClient();
        var vm = new GroupsViewModel(fake);

        // Act
        await vm.LoadGroupsAsync();

        // Assert
        Assert.True(fake.WasCalled);
        Assert.Equal(2, vm.Groups.Count);
        Assert.Equal("Loaded 2 groups.", vm.StatusMessage);
    }
}

