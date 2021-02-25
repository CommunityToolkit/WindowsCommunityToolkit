// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// RangeSelector is a "double slider" control for range values.
    /// </summary>
    public partial class RangeSelector : Control
    {
        /// <summary>
        /// Event raised when lower or upper range values are changed.
        /// </summary>
        public event EventHandler<RangeChangedEventArgs> ValueChanged;

        /// <summary>
        /// Event raised when lower or upper range thumbs start being dragged.
        /// </summary>
        public event DragStartedEventHandler ThumbDragStarted;

        /// <summary>
        /// Event raised when lower or upper range thumbs end being dragged.
        /// </summary>
        public event DragCompletedEventHandler ThumbDragCompleted;

        private void OnValueChanged(RangeChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private void OnThumbDragStarted(DragStartedEventArgs e)
        {
            ThumbDragStarted?.Invoke(this, e);
        }

        private void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            ThumbDragCompleted?.Invoke(this, e);
        }
    }
}
