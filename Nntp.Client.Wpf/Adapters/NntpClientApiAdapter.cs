using Nntp.Client.Wpf.ViewModel;
using Nntp.Core.Client;
using Nntp.Core.Models;
using Nntp.Core.Net;
using Nntp.Core.Repository;


namespace Nntp.Client.Wpf;

/// <summary>
/// Bridges the WPF VM interface (INntpClientApi) to the real NntpClient over TCP.
/// It: loads settings → connects → optional authenticate → LIST.
/// </summary>
public sealed class NntpClientApiAdapter : INntpClientApi
{
    private readonly ISettingsRepository _repo;

    public NntpClientApiAdapter(ISettingsRepository repo) {_repo = repo;}

    private string? _lastGroupName; 

    public async Task<IReadOnlyList<NewsGroup>> ListGroupsAsync()
    {
        var settings = _repo.Load() ?? throw new InvalidOperationException("No saved settings.");
        using var transport = new TcpNntpTransport();
        var client = new NntpClient(transport);
        await client.ConnectAsync(settings.ServerName, settings.Port);
 
        if (!string.IsNullOrWhiteSpace(settings.Username))
            await transport.SendCommandAsync($"authinfo user {settings.Username}\r\n");
        if (!string.IsNullOrWhiteSpace(settings.Password))
            await transport.SendCommandAsync($"authinfo pass {settings.Password}\r\n");
        return await client.ListGroupsAsync();       
    }


    public async Task<IReadOnlyList<ArticleHead>> ListArticlesAsync(string groupName)
    {
        //Remember last group for later ARTICLE calls
        _lastGroupName = groupName;

        var settings = _repo?.Load() ?? throw new InvalidOperationException("No saved settings.");
        using var transport = new TcpNntpTransport();
        var client = new NntpClient(transport);
        await client.ConnectAsync(settings.ServerName, settings.Port);

        if (!string.IsNullOrWhiteSpace(settings.Username))
            await transport.SendCommandAsync($"authinfo user {settings.Username}\r\n");
        if (!string.IsNullOrWhiteSpace(settings.Password))
            await transport.SendCommandAsync($"authinfo pass {settings.Password}\r\n");
        return await client.ListArticlesAsync(groupName);
    }

    public async Task<string>GetArticleAsync(int articleNumber)
    {
        var settings = _repo.Load() ?? throw new InvalidOperationException("No saved settings.");
        using var transport = new TcpNntpTransport(); 
        var client = new NntpClient(transport);
        await client.ConnectAsync(settings.ServerName, settings.Port); 

        if(!string.IsNullOrWhiteSpace(settings.Username))
            await transport.SendCommandAsync($"authinfo user {settings.Username}\r\n");
        if (!string.IsNullOrWhiteSpace(settings.Password))
            await transport.SendCommandAsync($"authinfo pass {settings.Password}\r\n");
        
        //ensure a group is selected for ARTICLE
        if(!string.IsNullOrWhiteSpace(_lastGroupName))
            _=await transport.SendCommandAsync($"GROUP {_lastGroupName}\r\n");

        return await client.GetArticleAsync(articleNumber);
    }
}
