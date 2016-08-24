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
        private ScrollViewer _scrollViewer;
        private double _previousVerticalScrollOffset;
        private CompositionPropertySet _scrollProperties;
        private CompositionPropertySet _animationProperties;
        private Visual _headerVisual;

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

        /// <summary>
        /// Show the header
        /// </summary>
        public void Show()
        {
            if (_headerVisual != null && _scrollViewer != null)
            {
                FrameworkElement header = (FrameworkElement)TargetListView.Header;
                _animationProperties.InsertScalar("OffsetY", 0.0f);
            }
        }

        protected override void OnApplyTemplate()
        {
            SizeChanged -= QuickReturnHeader_SizeChanged;
            SizeChanged += QuickReturnHeader_SizeChanged;

            if (TargetListView != null)
            {
                _scrollViewer = GetScrollViewer(TargetListView);

                // Place items below header
                var panel = TargetListView.ItemsPanelRoot;
                Canvas.SetZIndex(panel, -1);
            }

            if (_scrollViewer != null)
            {
                _scrollViewer.ViewChanged -= ScrollViewer_ViewChanged;
                _scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
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
            if (_animationProperties != null)
            {
                float oldOffsetY = 0.0f;
                _animationProperties.TryGetScalar("OffsetY", out oldOffsetY);

                var delta = _scrollViewer.VerticalOffset - _previousVerticalScrollOffset;
                _previousVerticalScrollOffset = _scrollViewer.VerticalOffset;

                var newOffsetY = oldOffsetY - (float)delta;

                // Keep values within negativ header size and 0
                FrameworkElement header = (FrameworkElement)TargetListView.Header;
                newOffsetY = Math.Max((float)-header.ActualHeight, newOffsetY);
                newOffsetY = Math.Min(0, newOffsetY);

                if (oldOffsetY != newOffsetY)
                {
                    _animationProperties.InsertScalar("OffsetY", newOffsetY);
                }
            }
        }

        private void StartAnimation()
        {
            if (_scrollProperties == null)
            {
                _scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
            }

            var compositor = _scrollProperties.Compositor;

            if (_animationProperties == null)
            {
                _animationProperties = compositor.CreatePropertySet();
                _animationProperties.InsertScalar("OffsetY", 0.0f);
            }

            var expressionAnimation = compositor.CreateExpressionAnimation("Floor(animationProperties.OffsetY - ScrollingProperties.Translation.Y)");

            expressionAnimation.SetReferenceParameter("ScrollingProperties", _scrollProperties);
            expressionAnimation.SetReferenceParameter("animationProperties", _animationProperties);

            _headerVisual = ElementCompositionPreview.GetElementVisual((UIElement)TargetListView.Header);

            if (_headerVisual != null && IsQuickReturnEnabled)
            {
                _headerVisual.StartAnimation("Offset.Y", expressionAnimation);
            }
        }

        private void StopAnimation()
        {
            if (_headerVisual != null)
            {
                _headerVisual.StopAnimation("Offset.Y");
                _animationProperties.InsertScalar("OffsetY", 0.0f);

                var offset = _headerVisual.Offset;
                offset.Y = 0.0f;
                _headerVisual.Offset = offset;
            }
        }
    }
}
