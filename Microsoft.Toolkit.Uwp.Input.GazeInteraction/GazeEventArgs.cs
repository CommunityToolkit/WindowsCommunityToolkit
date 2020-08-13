// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    /// <summary>
    /// EventArgs used to send Gaze events. See <see cref="GazePointer.GazeEvent"/>
    /// </summary>
    public class GazeEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the application handled the event. If this parameter is set to true, the library prevents handling the events on that particular gaze event.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the location of the Gaze event
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the gaze event
        /// </summary>
        public TimeSpan Timestamp { get; set; }

        internal void Set(Point location, TimeSpan timestamp)
        {
            Handled = false;
            Location = location;
            Timestamp = timestamp;
        }
    }
}
