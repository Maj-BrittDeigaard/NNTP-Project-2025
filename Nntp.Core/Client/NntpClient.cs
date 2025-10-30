using Nntp.Core.Models;
using Nntp.Core.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nntp.Core.Client
{
    /// <summary>
    /// Provides NNTP operations such as LIST to retrieve newsgroups.
    /// </summary>
    public sealed class NntpClient
    {
        private readonly INntpTransport _transport;

        public NntpClient(INntpTransport transport)
        {
            _transport = transport;
        }

        // For later use when real TCP connection is implemented
        public Task ConnectAsync(string host, int port)
            => _transport.ConnectAsync(host, port);

        /// <summary>
        /// Sends LIST command and parses server response into NewsGroup objects.
        /// </summary>
        public async Task<IReadOnlyList<NewsGroup>> ListGroupsAsync()
        {
            // Send LIST command to NNTP server
            _ = await _transport.SendCommandAsync("LIST\r\n");

            // Read all lines from the server until "."
            var lines = await _transport.ReadMultilineAsync();

            // Parse each line into NewsGroup objects
            var result = new List<NewsGroup>(lines.Count);
            foreach (var line in lines)
            {
                if (TryParseGroupLine(line, out var group))
                    result.Add(group!);
            }
            return result;
        }

        /// <summary>
        /// Helper method to parse a single line from the LIST response.
        /// Format: group.name high low postingflag
        /// </summary>
        private static bool TryParseGroupLine(string line, out NewsGroup? group)
        {
            group = null;

            if (string.IsNullOrWhiteSpace(line) || line == ".")
                return false;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 4)
                return false;

            _ = int.TryParse(parts[1], out var high);
            _ = int.TryParse(parts[2], out var low);
            var flag = parts[3][0];

            group = new NewsGroup
            {
                Name = parts[0],
                HighWatermark = high,
                LowWatermark = low,
                PostingFlag = flag
            };
            return true;
        }

        public async Task<IReadOnlyList<ArticleHead>> ListArticlesAsync(string groupName)
        {
            //Select the group 
            _=await _transport.SendCommandAsync($"GROUP {groupName}\r\n");

            //Request overview (headlines)
            _ = await _transport.SendCommandAsync("XOVER 1-\r\n");

            //Read the multiline overview block terminated by a single dot 
            var lines = await _transport.ReadMultilineAsync();

            var result = new List<ArticleHead>(lines.Count);
            foreach (var line in lines) 
            {
                if (TryParseArticleLine(line, out var head))
                    result.Add(head!);
            }
            return result;
        }

        public static bool TryParseArticleLine(string line, out ArticleHead? head)
        {
            head = null;
            if (string.IsNullOrWhiteSpace(line) || line == ".") return false;

            var parts = line.Split('\t');
            if (parts.Length < 4 ) return false;

            _=int.TryParse(parts[0], out var num);
            head = new ArticleHead
            {
                Number = num,
                Subject = parts[1],
                From = parts[2],
                Date = parts[3],
            };
            return true;
        }

        public async Task<string> GetArticleAsync (int articleNumber)
        {
            //Send ARTICLE command for the selected headline
            _ = await _transport.SendCommandAsync($"ARTICLE {articleNumber}\r\n"); 

            //Read multi-line respones (header + body, terminated by ".") 
            var lines = await _transport.ReadMultilineAsync();

            //Join into a single string for display
            return string.Join("\n", lines);
        }
    }
}
