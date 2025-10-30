using Nntp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nntp.Client.Wpf.ViewModel
{
    /// <summary>
    /// ViewModel for headlines (XOVER) and the selected full article (ARTICLE) 
    /// </summary>
    public sealed class HeadlinesViewModel : INotifyPropertyChanged
    {
        private readonly INntpClientApi _api;

        public ObservableCollection<ArticleHead> Articles { get; } = new();

        private string _status = "Ready";
        public string StatusMessage
        {
            get => _status;
            private set { _status = value; OnPropertyChanged(); }
        }

        private string _articleText = ""; 
        public string ArticleText 
        {
            get => _articleText;
            private set { _articleText = value; OnPropertyChanged(); }
        }

        public HeadlinesViewModel(INntpClientApi api) => _api = api;

        public async Task LoadArticlesAsync(string groupName)
        {
            StatusMessage = $"Loading articles for {groupName}..";
            var data = await _api.ListArticlesAsync(groupName);
            Articles.Clear();
            foreach (var a in data) Articles.Add(a);
            StatusMessage = $"Loaded {Articles.Count} articles. ";
            //Clear right panel when group changes
            ArticleText = ""; 
        }

        //Load full article text by article number
        public async Task LoadArticleAsync (int articleNumber) 
        {
            StatusMessage = $"Loading article {articleNumber}..";
            try
            {
                ArticleText = await _api.GetArticleAsync(articleNumber);
                StatusMessage = "Article loaded.";
            }
            catch (Exception ex) 
            {
                ArticleText = $"Error loading article: {ex.Message}";
                StatusMessage = "Error.";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
