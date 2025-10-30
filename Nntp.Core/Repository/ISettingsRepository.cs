using Nntp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nntp.Core.Repository
{
    /// <summary>
    /// Interface that defines methods for saving and loading server settings.
    /// </summary>
    public interface ISettingsRepository
    {
        void Save(ServerSettings settings);
        ServerSettings? Load();
    }
}
