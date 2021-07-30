// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services.LinkedIn
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

#pragma warning disable SA1649 // File name should match first type name
    internal static class LinkedInPermissionsHelpers
    {
        /// <summary>
        /// Internal AllPermissions for LinkedInPermissions, so we don't expose it. Keep it in sync with <see cref="LinkedInPermissions"/>
        /// </summary>
        internal const LinkedInPermissions AllPermissions =
            LinkedInPermissions.ReadBasicProfile |
            LinkedInPermissions.ReadEmailAddress |
            LinkedInPermissions.ReadWriteCompanyAdmin |
            LinkedInPermissions.WriteShare;
    }
#pragma warning restore SA1649 // File name should match first type name
}
