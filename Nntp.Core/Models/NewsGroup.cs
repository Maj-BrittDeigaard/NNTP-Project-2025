using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nntp.Core.Models
{
    public sealed class NewsGroup
    {
        public string Name { get; init; } = ""; 
        public int HighWatermark { get; init; } 
        public int LowWatermark { get; init; }
        public char PostingFlag { get; init; } 
    }
}
