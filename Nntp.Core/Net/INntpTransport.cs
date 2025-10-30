using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nntp.Core.Net
{
    public interface INntpTransport
    {
        //Establishes the TCP connection (real impl later, fake in tests) 
        Task ConnectAsync(string host, int port);
        //Sends a single command and returns the status line 
        Task<string> SendCommandAsync(string command);
        //Reads a multi-line response until a single "." line is encountered 
        Task<IReadOnlyList<string>> ReadMultilineAsync(); 
    }
}
