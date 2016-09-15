using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// List of user related data permissions
    /// </summary>
    [Flags]
    public enum LinkedInShareVisibility
    {
        /// <summary>
        /// Connections only
        /// </summary>
        ConnectionsOnly = 1,

        /// <summary>
        /// Anyone
        /// </summary>
        Anyone = 2
    }
}
