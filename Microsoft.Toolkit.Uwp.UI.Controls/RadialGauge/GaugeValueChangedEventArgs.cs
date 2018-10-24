// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event args for a value changing event
    /// </summary>
    public class GaugeValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the old value.
        /// </summary>
        public double OldValue { get; private set; }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public double NewValue { get; private set; }

        /// <summary>
        /// Gets the old value angle.
        /// </summary>
        public double OldValueAngle { get; private set; }

        /// <summary>
        /// Gets the new value angle.
        /// </summary>
        public double NewValueAngle { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        /// <param name="oldvalueAngle">The old value angle</param>
        /// <param name="newvalueAngle">The new value angle</param>
        public GaugeValueChangedEventArgs(
                         double oldValue,
                         double newValue,
                         double oldvalueAngle,
                         double newvalueAngle)
        {
            OldValue = oldValue;
            NewValue = newValue;
            OldValueAngle = oldvalueAngle;
            NewValueAngle = newvalueAngle;
        }
    }
}
