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
    public enum LinkedInPermissions
    {
        /// <summary>
        /// Not set
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// Read - Basic profile
        /// </summary>
        R_BasicProfile = 1,

        /// <summary>
        /// Read - Email Address
        /// </summary>
        R_EmailAddress = 2,

        /// <summary>
        /// Read / Write - Company Admin
        /// </summary>
        RW_Company_Admin = 4,

        /// <summary>
        /// Write - Share
        /// </summary>
        W_Share = 8
    }
}
