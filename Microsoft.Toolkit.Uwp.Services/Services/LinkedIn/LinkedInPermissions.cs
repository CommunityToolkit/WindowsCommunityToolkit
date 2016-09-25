using System;

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
        /// Read - Basic profile (r_basicprofile)
        /// </summary>
        ReadBasicProfile = 1,

        /// <summary>
        /// Read - Email Address (r_emailaddress)
        /// </summary>
        ReadEmailAddress = 2,

        /// <summary>
        /// Read / Write - Company Admin (rw_company_admin)
        /// </summary>
        ReadWriteCompanyAdmin = 4,

        /// <summary>
        /// Write - Share (w_share)
        /// </summary>
        WriteShare = 8
    }
}
