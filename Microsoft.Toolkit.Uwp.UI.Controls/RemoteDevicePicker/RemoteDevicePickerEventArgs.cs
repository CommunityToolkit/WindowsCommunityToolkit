// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using Windows.System.RemoteSystems;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arguments for the RemoteDevicePickerClosed event to show List of devices selected
    /// </summary>
    public class RemoteDevicePickerEventArgs : EventArgs
    {
        /// <summary>
        /// Gets List of Devices that are selected
        /// </summary>
        public ObservableCollection<RemoteSystem> Devices { get; }

        internal RemoteDevicePickerEventArgs(ObservableCollection<RemoteSystem> devices)
        {
            Devices = devices;
        }
    }
}
