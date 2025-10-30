using System.Collections.Generic;
using System.Threading.Tasks;
using Nntp.Core.Models;

namespace Nntp.Client.Wpf.ViewModel
{
    /// <summary>
    /// Minimal contract that the GUI depends on for listing newsgroups.
    /// Keeps the ViewModel decoupled from any concrete NNTP client.
    /// </summary>
    public interface INntpClientApi
    {
        Task<IReadOnlyList<NewsGroup>> ListGroupsAsync();
        Task<IReadOnlyList<ArticleHead>> ListArticlesAsync(string groupName);
        Task<string> GetArticleAsync(int articleNumber);
    }
}
