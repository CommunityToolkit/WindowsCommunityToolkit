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
using System.Net;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    internal static class Security
    {
        private static SecurityPermission _unmanagedCodePermission;

        private static WebBrowserPermission _webBrowserPermission;

        internal static WebBrowserPermission CachedWebBrowserPermission
        {
            [SecurityCritical]
            get
            {
                return _webBrowserPermission ?? (_webBrowserPermission = new WebBrowserPermission(PermissionState.Unrestricted));
            }
        }

        [SecuritySafeCritical]
        internal static bool AppDomainHasPermission(IPermission permissionToCheck)
        {
            var psToCheck = new PermissionSet(PermissionState.None);
            psToCheck.AddPermission(permissionToCheck);
            return psToCheck.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }

        [SecuritySafeCritical]
        internal static bool CallerAndAppDomainHaveUnrestrictedWebBrowserPermission()
        {
            if (!AppDomainHasPermission(CachedWebBrowserPermission))
            {
                return false;
            }

            try
            {
                DemandWebBrowserPermission();
            }
            catch (SecurityException)
            {
                return false;
            }

            return true;
        }

        [SecurityCritical]
        internal static bool CallerHasWebPermission(Uri uri)
        {
            try
            {
                DemandWebPermission(uri);
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        // Exception raised by a demand may contain security sensitive information

        /// <exception cref="SecurityException">A caller higher in the call stack does not have the permission specified by the current instance.-or- A caller higher in the call stack has called <see cref="M:System.Security.CodeAccessPermission.Deny" /> on the current permission object. </exception>
        [SecurityCritical]
        internal static void DemandUnamangedCode()
        {
            if (_unmanagedCodePermission == null)
            {
                _unmanagedCodePermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
            }

            _unmanagedCodePermission.Demand();
        }

        [SecurityCritical]
        internal static void DemandWebBrowserPermission()
        {
            CachedWebBrowserPermission.Demand();
        }

        [SecurityCritical] // Exceptions raised by a demand may contain security sensisitve information
        internal static void DemandWebPermission(Uri uri)
        {
            if (uri == null)
            {
                return;
            }

            if (uri.IsFile)
            {
                new FileIOPermission(FileIOPermissionAccess.Read, uri.LocalPath).Demand();
            }
            else
            {
                // As a security measure, since the code below uses a string anyway,
                // sanitize the input
                var finalUri = UriHelper.UriToString(uri);

                new WebPermission(NetworkAccess.Connect, finalUri).Demand();
            }
        }
    }
}