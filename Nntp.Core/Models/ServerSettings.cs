using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nntp.Core.Models
{
    /// <summary>
    ///  This class represents the data entered by the user in the GUI
    // (server name, port, username, password).
    // It serves as a model that will later be saved to a JSON file.   
    /// </summary>
    public class ServerSettings
    {
        public string ServerName { get; set; } = "";
        public int Port { get; set; } = 119;
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
