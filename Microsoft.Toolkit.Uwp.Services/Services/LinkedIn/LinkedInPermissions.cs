// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
