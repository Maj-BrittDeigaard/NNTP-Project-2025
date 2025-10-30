using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nntp.Core.Models
{
    /// <summary>
    /// Represents a single article headline from the NNTP XOVER response
    /// </summary>
    public sealed class ArticleHead
    {
        public int Number { get; init; }
        public string Subject { get; init; } = "";
        public string From { get; init; } = "";
        public string Date { get; init; } = "";
    }
}
