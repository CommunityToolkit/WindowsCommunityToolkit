// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    /// <summary>
    /// This parameter is passed to the GazeElement.DwellProgressFeedback event. The event is fired to inform the application of the user's progress towards completing dwelling on a control
    /// </summary>
    public class DwellProgressEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets an enum that reflects the current state of dwell progress
        /// </summary>
        public DwellProgressState State { get; }

        /// <summary>
        /// Gets a value between 0 and 1 that reflects the fraction of progress towards completing dwell
        /// </summary>
        public double Progress { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the application handled the event. If this parameter is set to true, the library suppresses default animation for dwell feedback on the control
        /// </summary>
        public bool Handled { get; set; }

        internal DwellProgressEventArgs(DwellProgressState state, TimeSpan elapsedDuration, TimeSpan triggerDuration)
        {
            State = state;
            Progress = ((double)elapsedDuration.Ticks) / triggerDuration.Ticks;
        }
    }
}