using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Store data in the Roaming environment (shared around all user devices)
    /// </summary>
    public class RoamingSettingStorageService : BaseSettingStorageService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoamingSettingStorageService"/> class.
        /// </summary>
        public RoamingSettingStorageService()
        {
            Settings = ApplicationData.Current.RoamingSettings;
            Folder = ApplicationData.Current.RoamingFolder;
        }
    }
}
