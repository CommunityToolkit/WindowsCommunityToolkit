// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        // Critical: Exception raised by a demand may contain security sensitive information

        /// <summary>
        /// Demands <see cref="SecurityPermission"/> for <see cref="SecurityPermissionFlag.UnmanagedCode"/>
        /// </summary>
        /// <exception cref="SecurityException">A caller higher in the call stack does not have the permission specified by the current instance.-or- A caller higher in the call stack has called <see cref="M:System.Security.CodeAccessPermission.Deny" /> on the current permission object.</exception>
        [SecurityCritical]
        internal static void DemandUnmanagedCode()
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

        // Critical: Exceptions raised by a demand may contain security sensitive information
        [SecurityCritical]
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