using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Store data in the Local environment (only on the current device)
    /// </summary>
    public class LocalStorageService : BaseStorageService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalStorageService"/> class.
        /// </summary>
        public LocalStorageService()
        {
            Settings = ApplicationData.Current.LocalSettings;
            Folder = ApplicationData.Current.LocalFolder;
        }
    }
}
