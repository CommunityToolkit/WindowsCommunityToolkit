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
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Quick return header control to be used with ListViews
    /// </summary>
    public class QuickReturnHeader : ContentControl
    {
        public QuickReturnHeader()
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// Identifies the <see cref="IsQuickReturnEnabled"/> property.
        /// </summary>
        public static readonly DependencyProperty IsQuickReturnEnabledProperty =
            DependencyProperty.Register(nameof(IsQuickReturnEnabled), typeof(bool), typeof(QuickReturnHeader), new PropertyMetadata(true, OnIsQuickReturnEnabledChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the quick return header is enabled.
        /// If true the quick return behavior is used.
        /// If false regular header behavior is used.
        /// Default is true.
        /// </summary>
        public bool IsQuickReturnEnabled
        {
            get { return (bool)GetValue(IsQuickReturnEnabledProperty); }
            set { SetValue(IsQuickReturnEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ListView this header belongs to
        /// </summary>
        public ListView TargetListView { get; set; }

        protected override void OnApplyTemplate()
        {
            SizeChanged -= QuickReturnHeader_SizeChanged;
            SizeChanged += QuickReturnHeader_SizeChanged;

            if (TargetListView != null)
            {
                scrollViewer = GetScrollViewer(TargetListView);

                // Place items below header
                var panel = TargetListView.ItemsPanelRoot;
                Canvas.SetZIndex(panel, -1);
            }

            if (scrollViewer != null)
            {
                scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
                scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            }
        }

        private static ScrollViewer GetScrollViewer(DependencyObject o)
        {
            if (o is ScrollViewer)
            {
                return o as ScrollViewer;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetScrollViewer(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private static void OnIsQuickReturnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as QuickReturnHeader;

            if (me.IsQuickReturnEnabled)
            {
                me.StartAnimation();
            }
            else
            {
                me.StopAnimation();
            }
        }

        private void QuickReturnHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (TargetListView != null)
            {
                if (IsQuickReturnEnabled)
                {
                    StartAnimation();
                }
            }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (animationProperties != null)
            {
                float oldOffsetY = 0.0f;
                animationProperties.TryGetScalar("OffsetY", out oldOffsetY);

                var delta = scrollViewer.VerticalOffset - previousVerticalScrollOffset;
                previousVerticalScrollOffset = scrollViewer.VerticalOffset;

                var newOffsetY = oldOffsetY - (float)delta;

                // Keep values within negativ header size and 0
                FrameworkElement header = (FrameworkElement)TargetListView.Header;
                newOffsetY = Math.Max((float)-header.ActualHeight, newOffsetY);
                newOffsetY = Math.Min(0, newOffsetY);

                if (oldOffsetY != newOffsetY)
                {
                    animationProperties.InsertScalar("OffsetY", newOffsetY);
                }
            }
        }

        private void StartAnimation()
        {
            if (scrollProperties == null)
            {
                scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollViewer);
            }

            var compositor = scrollProperties.Compositor;

            if (animationProperties == null)
            {
                animationProperties = compositor.CreatePropertySet();
                animationProperties.InsertScalar("OffsetY", 0.0f);
            }

            var expressionAnimation = compositor.CreateExpressionAnimation("Floor(animationProperties.OffsetY - ScrollingProperties.Translation.Y)");

            expressionAnimation.SetReferenceParameter("ScrollingProperties", scrollProperties);
            expressionAnimation.SetReferenceParameter("animationProperties", animationProperties);

            headerVisual = ElementCompositionPreview.GetElementVisual((UIElement)TargetListView.Header);

            if (headerVisual != null && IsQuickReturnEnabled)
            {
                headerVisual.StartAnimation("Offset.Y", expressionAnimation);
            }
        }

        private void StopAnimation()
        {
            if (headerVisual != null)
            {
                headerVisual.StopAnimation("Offset.Y");
                animationProperties.InsertScalar("OffsetY", 0.0f);

                var offset = headerVisual.Offset;
                offset.Y = 0.0f;
                headerVisual.Offset = offset;
            }
        }

        private ScrollViewer scrollViewer;
        private double previousVerticalScrollOffset;
        private CompositionPropertySet scrollProperties;
        private CompositionPropertySet animationProperties;
        private Visual headerVisual;
    }
}
