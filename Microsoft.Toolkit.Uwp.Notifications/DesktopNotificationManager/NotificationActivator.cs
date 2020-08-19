// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Windows.UI.Notifications;

namespace Microsoft.Toolkit.Uwp.Notifications.Internal
{
    /// <summary>
    /// Apps must implement this activator to handle notification activation.
    /// </summary>
    public abstract class NotificationActivator : NotificationActivator.INotificationActivationCallback
    {
        /// <inheritdoc/>
        public void Activate(string appUserModelId, string invokedArgs, NOTIFICATION_USER_INPUT_DATA[] data, uint dataCount)
        {
            DesktopNotificationManagerCompat.OnActivatedInternal(invokedArgs, data, appUserModelId);
        }

        /// <summary>
        /// A single user input key/value pair.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct NOTIFICATION_USER_INPUT_DATA
        {
            /// <summary>
            /// The key of the user input.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Key;

            /// <summary>
            /// The value of the user input.
            /// </summary>
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Value;
        }

        /// <summary>
        /// The COM callback that is triggered when your notification is clicked.
        /// </summary>
        [ComImport]
        [Guid("53E31837-6600-4A81-9395-75CFFE746F94")]
        [ComVisible(true)]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface INotificationActivationCallback
        {
            /// <summary>
            /// The method called when your notification is clicked.
            /// </summary>
            /// <param name="appUserModelId">The app id of the app that sent the toast.</param>
            /// <param name="invokedArgs">The activation arguments from the toast.</param>
            /// <param name="data">The user input from the toast.</param>
            /// <param name="dataCount">The number of user inputs.</param>
            void Activate(
                [In, MarshalAs(UnmanagedType.LPWStr)]
                string appUserModelId,
                [In, MarshalAs(UnmanagedType.LPWStr)]
                string invokedArgs, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] NOTIFICATION_USER_INPUT_DATA[] data,
                [In, MarshalAs(UnmanagedType.U4)]
                uint dataCount);
        }
    }
}