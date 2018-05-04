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
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Scroll header control to be used with ListViews or GridViews
    /// </summary>
    public class ScrollHeader : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollHeader"/> class.
        /// </summary>
        public ScrollHeader()
        {
            DefaultStyleKey = typeof(ScrollHeader);
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// Identifies the <see cref="Mode"/> property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(ScrollHeaderMode), typeof(ScrollHeader), new PropertyMetadata(ScrollHeaderMode.None, OnModeChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the current mode.
        /// Default is none.
        /// </summary>
        public ScrollHeaderMode Mode
        {
            get { return (ScrollHeaderMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="Control.ApplyTemplate"/>.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            UpdateScrollHeaderBehavior();
        }

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollHeader)?.UpdateScrollHeaderBehavior();
        }

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollHeader)?.OnApplyTemplate();
        }

        private void UpdateScrollHeaderBehavior()
        {
            var targetListViewBase = this.FindAscendant<Windows.UI.Xaml.Controls.ListViewBase>();

            if (targetListViewBase == null)
            {
                return;
            }

            // Remove previous behaviors
            foreach (var behavior in Interaction.GetBehaviors(targetListViewBase))
            {
                if (behavior is FadeHeaderBehavior || behavior is QuickReturnHeaderBehavior || behavior is StickyHeaderBehavior)
                {
                    Interaction.GetBehaviors(targetListViewBase).Remove(behavior);
                }
            }

            switch (Mode)
            {
                case ScrollHeaderMode.None:
                    break;
                case ScrollHeaderMode.QuickReturn:
                    Interaction.GetBehaviors(targetListViewBase).Add(new QuickReturnHeaderBehavior());
                    break;
                case ScrollHeaderMode.Sticky:
                    Interaction.GetBehaviors(targetListViewBase).Add(new StickyHeaderBehavior());
                    break;
                case ScrollHeaderMode.Fade:
                    Interaction.GetBehaviors(targetListViewBase).Add(new FadeHeaderBehavior());
                    break;
            }
        }
    }
}
