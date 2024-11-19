// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A thumb that represents a value within a range.
    /// </summary>
    [TemplatePart(Name = "InternalThumb", Type = typeof(Thumb))]
    public class RangeThumb : RangeBase
    {
        private Thumb _thumb;

        /// <inheritdoc cref="Thumb.DragStarted"/>
        public event EventHandler<DragStartedEventArgs> DragStarted;

        /// <inheritdoc cref="Thumb.DragCompleted"/>
        public event EventHandler<DragCompletedEventArgs> DragCompleted;

        /// <inheritdoc cref="Thumb.DragDelta"/>
        public event EventHandler<DragDeltaEventArgs> DragDelta;

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _thumb = GetTemplateChild("InternalThumb") as Thumb;

            if (_thumb is not null)
            {
                AttachEvents(_thumb);
            }

            Unloaded += RangeThumb_Unloaded;

            base.OnApplyTemplate();
        }

        private void AttachEvents(Thumb thumb)
        {
            thumb.DragCompleted += Thumb_DragCompleted;
            thumb.DragStarted += Thumb_DragStarted;
            thumb.DragDelta += Thumb_DragDelta;
        }

        private void DetachEvents(Thumb thumb)
        {
            thumb.DragCompleted -= Thumb_DragCompleted;
            thumb.DragStarted -= Thumb_DragStarted;
            thumb.DragDelta -= Thumb_DragDelta;
        }

        private void Thumb_DragStarted(object sender, DragStartedEventArgs e) => DragStarted?.Invoke(this, e);

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e) => DragDelta?.Invoke(this, e);

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e) => DragCompleted?.Invoke(this, e);

        private void RangeThumb_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_thumb is not null)
            {
                DetachEvents(_thumb);
            }
        }

        /// <inheritdoc/>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new RangeThumbAutomationPeer(this);
        }
    }
}