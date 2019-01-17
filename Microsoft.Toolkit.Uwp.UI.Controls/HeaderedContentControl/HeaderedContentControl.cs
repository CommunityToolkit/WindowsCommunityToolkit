// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides the base implementation for all controls that contain single content and have a header.
    /// </summary>
    public class HeaderedContentControl : ContentControl
    {
        private const string PartHeaderPresenter = "HeaderPresenter";

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderedContentControl"/> class.
        /// </summary>
        public HeaderedContentControl()
        {
            DefaultStyleKey = typeof(HeaderedContentControl);
            GotFocus += HeaderedContentControl_GotFocus;
            LostFocus += HeaderedContentControl_LostFocus;
            KeyDown += HeaderedContentControl_KeyDown;
        }

        private void HeaderedContentControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            DependencyObject candidate = null;

            var options = new FindNextElementOptions()
            {
                SearchRoot = VisualTreeHelper.GetParent(this)
            };

            switch (e.Key)
            {
                case Windows.System.VirtualKey.Up:
                    candidate =
                        FocusManager.FindNextElement(
                            FocusNavigationDirection.Up, options);
                    break;
                case Windows.System.VirtualKey.Down:
                    candidate =
                        FocusManager.FindNextElement(
                            FocusNavigationDirection.Down, options);
                    break;
                case Windows.System.VirtualKey.Left:
                    candidate = FocusManager.FindNextElement(
                        FocusNavigationDirection.Left, options);
                    break;
                case Windows.System.VirtualKey.Right:
                    candidate =
                        FocusManager.FindNextElement(
                            FocusNavigationDirection.Right, options);
                    break;
            }

            if (candidate != null && candidate is Control)
            {
                (candidate as Control).Focus(FocusState.Keyboard);
            }
        }

        private void HeaderedContentControl_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Focused", false);

        }

        private void HeaderedContentControl_LostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Unfocused", false);
        }


        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(object),
            typeof(HeaderedContentControl),
            new PropertyMetadata(null, OnHeaderChanged));

        /// <summary>
        /// Identifies the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate",
            typeof(DataTemplate),
            typeof(HeaderedContentControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(HeaderedContentControl),
            new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));

        /// <summary>
        /// Gets or sets the <see cref="Orientation"/> used for the header.
        /// </summary>
        /// <remarks>
        /// If set to <see cref="Orientation.Vertical"/> the header will be above the content.
        /// If set to <see cref="Orientation.Horizontal"/> the header will be to the left of the content.
        /// </remarks>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the data used for the header of each control.
        /// </summary>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template used to display the content of the control's header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetHeaderVisibility();
        }

        /// <summary>
        /// Called when the <see cref="Header"/> property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the <see cref="Header"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Header"/> property.</param>
        protected virtual void OnHeaderChanged(object oldValue, object newValue)
        {
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HeaderedContentControl)d;

            var orientation = control.Orientation == Orientation.Vertical
                ? nameof(Orientation.Vertical)
                : nameof(Orientation.Horizontal);

            VisualStateManager.GoToState(control, orientation, true);
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HeaderedContentControl)d;
            control.SetHeaderVisibility();
            control.OnHeaderChanged(e.OldValue, e.NewValue);
        }

        private void SetHeaderVisibility()
        {
            if (GetTemplateChild(PartHeaderPresenter) is FrameworkElement headerPresenter)
            {
                headerPresenter.Visibility = Header != null
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }
    }
}