// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    /// <summary>
    /// Gaze optimized scrollbar
    /// </summary>
    public sealed partial class GazeScrollbar : UserControl
    {
        private ScrollViewer _scrollViewer;

        /// <summary>
        /// Idetifies the Orientation dependency property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation",  typeof(Orientation), typeof(GazeScrollbar), null);

        /// <summary>
        /// Gets or sets the Orientation of the gaze optimized scrollbar
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Identifies the HorizontalPosition dependency property
        /// </summary>
        private static readonly DependencyProperty HorizontalPositionProperty =
            DependencyProperty.Register("HorizontalPosition", typeof(string), typeof(GazeScrollbar), null);

        /// <summary>
        /// Gets or sets the percentage of the scrollable content that is to the left of the viewport
        /// </summary>
        private string HorizontalPosition
        {
            get { return (string)GetValue(HorizontalPositionProperty); }
            set { SetValue(HorizontalPositionProperty, value); }
        }

        /// <summary>
        /// Identifies the VerticalPosition dependency property
        /// </summary>
        private static readonly DependencyProperty VerticalPositionProperty =
            DependencyProperty.Register("VerticalPosition", typeof(string), typeof(GazeScrollbar), null);

        /// <summary>
        /// Gets or sets the percentage of the scrollable content that is to the left of the viewport
        /// </summary>
        private string VerticalPosition
        {
            get { return (string)GetValue(VerticalPositionProperty); }
            set { SetValue(VerticalPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the line height for scrolling with the gaze optimized scrollbar
        /// </summary>
        public double LineHeight { get; set; }

        /// <summary>
        /// Gets or sets the line width for scrolling with the gaze optimized scrollbar
        /// </summary>
        public double LineWidth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GazeScrollbar"/> class.
        /// </summary>
        public GazeScrollbar()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Attaches this scrollbar to the given ScrollViewer and enables controlling the scroll viewer
        /// by clicking the buttons on this scrollbar
        /// </summary>
        /// <param name="scrollViewer">The ScrollViewer to attach to</param>
        public void AttachTo(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
            _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            _scrollViewer.ViewChanged += this.OnScrollViewerViewChanged;
            _scrollViewer.Loaded += OnScrollViewerLoaded;
        }

        private void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            if (Orientation == Orientation.Horizontal)
            {
                HorizontalScrollbar.Visibility = Visibility.Visible;
            }
            else
            {
                VerticalScrollbar.Visibility = Visibility.Visible;
            }

            HorizontalPosition = "0%";
            VerticalPosition = "0%";
        }

        private void OnScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (Orientation == Orientation.Horizontal)
            {
                HorizontalPosition = $"{(int)(_scrollViewer.HorizontalOffset / _scrollViewer.ExtentWidth * 100)}%";
            }
            else
            {
                VerticalPosition = $"{(int)(_scrollViewer.VerticalOffset / _scrollViewer.ExtentHeight * 100)}%";
            }
        }

        private void OnLineUpClicked(object sender, RoutedEventArgs e)
        {
            var delta = LineHeight > 0 ? LineHeight : 1;
            _scrollViewer.ChangeView(null, _scrollViewer.VerticalOffset - delta, null);
        }

        private void OnPageUpClicked(object sender, RoutedEventArgs e)
        {
            var delta = _scrollViewer.ViewportHeight;
            _scrollViewer.ChangeView(null, _scrollViewer.VerticalOffset - delta, null);
        }

        private void OnPageDownClicked(object sender, RoutedEventArgs e)
        {
            var delta = _scrollViewer.ViewportHeight;
            _scrollViewer.ChangeView(null, _scrollViewer.VerticalOffset + delta, null);
        }

        private void OnLineDownClicked(object sender, RoutedEventArgs e)
        {
            var delta = LineHeight > 0 ? LineHeight : 1;
            _scrollViewer.ChangeView(null, _scrollViewer.VerticalOffset + delta, null);
        }

        private void OnLineLeftClicked(object sender, RoutedEventArgs e)
        {
            var delta = LineWidth > 0 ? LineWidth : 1;
            _scrollViewer.ChangeView(_scrollViewer.HorizontalOffset - delta, null, null);
        }

        private void OnPageLeftClicked(object sender, RoutedEventArgs e)
        {
            var delta = _scrollViewer.ViewportWidth;
            _scrollViewer.ChangeView(_scrollViewer.HorizontalOffset - delta, null, null);
        }

        private void OnPageRightClicked(object sender, RoutedEventArgs e)
        {
            var delta = _scrollViewer.ViewportWidth;
            _scrollViewer.ChangeView(_scrollViewer.HorizontalOffset + delta, null, null);
        }

        private void OnLineRightClicked(object sender, RoutedEventArgs e)
        {
            var delta = LineWidth > 0 ? LineWidth : 1;
            _scrollViewer.ChangeView(_scrollViewer.HorizontalOffset + delta, null, null);
        }
    }
}
