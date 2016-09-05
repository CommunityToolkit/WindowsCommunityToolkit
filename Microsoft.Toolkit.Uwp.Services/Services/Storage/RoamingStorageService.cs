using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Services.Services.Storage
{
    /// <summary>
    /// Store data in the Roaming environment (shared around all user devices)
    /// </summary>
    public class RoamingStorageService : BaseStorageService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoamingStorageService"/> class.
        /// </summary>
        public RoamingStorageService()
        {
            Settings = ApplicationData.Current.RoamingSettings;
            Folder = ApplicationData.Current.RoamingFolder;
        }
    }
}
