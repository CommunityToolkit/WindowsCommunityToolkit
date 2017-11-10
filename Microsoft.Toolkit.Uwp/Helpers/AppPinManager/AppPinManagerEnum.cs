using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Enumeration listing all Pin results
    /// </summary>
    public enum PinResult
    {
        /// <summary>
        ///  Unsupported Device
        /// </summary>
        UnsupportedDevice = 0,

        /// <summary>
        ///  Unsupported Windows 10 OS ( Pin support Version StartMenu >= 15063 ,TaskBar >= 16299)
        /// </summary>
        UnsupportedOs = 1,

        /// <summary>
        /// pin access is denied
        /// </summary>
        PinNotAllowed = 2,

        /// <summary>
        /// App has added startMenu or TaskBar
        /// </summary>
        PinPresent = 3,

        /// <summary>
        /// App has already is avaliable in StartMenu orTaskBar
        /// </summary>
        PinAlreadyPresent = 4
    }
}
