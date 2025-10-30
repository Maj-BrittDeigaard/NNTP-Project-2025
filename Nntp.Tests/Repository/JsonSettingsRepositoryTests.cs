using Xunit; 
using Nntp.Core.Models;
using Nntp.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nntp.Tests.Repository;

public class JsonSettingsRepositoryTests
{
    private static string TempPath() =>
        Path.Combine(Path.GetTempPath(), "nntp_settings_test.json");

    [Fact]
    public void Save_And_Load_ShouldReturnSameValues() 
    {
        //Arrange
        var path = TempPath();
        if (File.Exists(path)) 
            File.Delete(path);

        ISettingsRepository repository = new JsonSettingsRepository(path);

        var input = new ServerSettings
        {
            ServerName = "news.sunsite.dk",
            Port = 119,
            Username = "testuser", //dummydata 
            Password = "testpass" //dummydata
        };

        //Act
        repository.Save(input);
        var loaded = repository.Load();

        //Assert
        Assert.NotNull(loaded);
        Assert.Equal("news.sunsite.dk", loaded!.ServerName);
        Assert.Equal(119, loaded.Port);
        Assert.Equal("testuser", loaded.Username);
        Assert.Equal("testpass", loaded.Password);
    }
}
