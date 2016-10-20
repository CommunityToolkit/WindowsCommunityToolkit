using System;

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
