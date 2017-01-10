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

using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Quick return header control to be used with ListViews or GridViews
    /// </summary>
    public class ScrollHeader : ContentControl
    {
        public ScrollHeader()
        {
            DefaultStyleKey = typeof(ScrollHeader);
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// Identifies the <see cref="QuickReturn"/> property.
        /// </summary>
        public static readonly DependencyProperty QuickReturnProperty =
            DependencyProperty.Register(nameof(QuickReturn), typeof(bool), typeof(ScrollHeader), new PropertyMetadata(false, OnQuickReturnChanged));

        /// <summary>
        /// Identifies the <see cref="Sticky"/> property.
        /// </summary>
        public static readonly DependencyProperty StickyProperty =
            DependencyProperty.Register(nameof(Sticky), typeof(bool), typeof(ScrollHeader), new PropertyMetadata(false, OnStickyChanged));

        /// <summary>
        /// Identifies the <see cref="Fade"/> property.
        /// </summary>
        public static readonly DependencyProperty FadeProperty =
            DependencyProperty.Register(nameof(Fade), typeof(bool), typeof(ScrollHeader), new PropertyMetadata(false, OnFadeChanged));

        /// <summary>
        /// Identifies the <see cref="TargetListViewBase"/> property.
        /// </summary>
        public static readonly DependencyProperty TargetListViewBaseProperty =
            DependencyProperty.Register(nameof(TargetListViewBase), typeof(ListViewBase), typeof(ScrollHeader), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the quick return header is enabled.
        /// If true the quick return behavior is used.
        /// If false regular header behavior is used.
        /// Default is true.
        /// </summary>
        public bool QuickReturn
        {
            get { return (bool)GetValue(QuickReturnProperty); }
            set { SetValue(QuickReturnProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the quick return header should always be visible.
        /// If true the header is always visible.
        /// If false the header will move out of view when scrolling down.
        /// Default is false.
        /// </summary>
        public bool Sticky
        {
            get { return (bool)GetValue(StickyProperty); }
            set { SetValue(StickyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether header fading is enabled.
        /// If true the header fades in and out.
        /// If false regular header behavior is used.
        /// Default is false.
        /// </summary>
        public bool Fade
        {
            get { return (bool)GetValue(FadeProperty); }
            set { SetValue(FadeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the container this header belongs to
        /// </summary>
        public ListView TargetListViewBase
        {
            get { return (ListView)GetValue(TargetListViewBaseProperty); }
            set { SetValue(TargetListViewBaseProperty, value); }
        }

        /// <summary>
        /// Show the header
        /// </summary>
        public void Show()
        {
            var scrollHeaderBehavior = GetScrollHeaderBehavior();

            if (scrollHeaderBehavior != null)
            {
                scrollHeaderBehavior.Show();
            }
        }

        protected override void OnApplyTemplate()
        {
            if (TargetListViewBase != null)
            {
                // Place items below header
                var panel = TargetListViewBase.ItemsPanelRoot;
                Canvas.SetZIndex(panel, -1);
            }
        }

        private static void OnQuickReturnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollHeader).UpdateScrollHeaderBehavior();
        }

        private static void OnStickyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollHeader).UpdateScrollHeaderBehavior();
        }

        private static void OnFadeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as ScrollHeader;

            if (me.TargetListViewBase != null)
            {
                if (me.Fade)
                {
                    var behavior = new FadeHeaderBehavior();

                    Interaction.GetBehaviors(me.TargetListViewBase).Add(behavior);
                }
                else
                {
                    foreach (var behavior in Interaction.GetBehaviors(me.TargetListViewBase))
                    {
                        if (behavior is FadeHeaderBehavior)
                        {
                            Interaction.GetBehaviors(me.TargetListViewBase).Remove(behavior);
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateScrollHeaderBehavior()
        {
            if (TargetListViewBase != null)
            {
                bool attachBehavior = false;
                ScrollHeaderBehavior behavior = GetScrollHeaderBehavior();

                if (behavior == null)
                {
                    behavior = new ScrollHeaderBehavior();
                    attachBehavior = true;
                }

                behavior.QuickReturn = QuickReturn;
                behavior.Sticky = Sticky;

                if (attachBehavior)
                {
                    Interaction.GetBehaviors(TargetListViewBase).Add(behavior);
                }
            }
        }

        private ScrollHeaderBehavior GetScrollHeaderBehavior()
        {
            foreach (var attachedBehavior in Interaction.GetBehaviors(TargetListViewBase))
            {
                if (attachedBehavior is ScrollHeaderBehavior)
                {
                    return attachedBehavior as ScrollHeaderBehavior;
                }
            }

            return null;
        }
    }
}
