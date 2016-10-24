﻿// ******************************************************************
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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    ///     MasterDetail control
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Control" />
    [TemplateVisualState(GroupName = DisplayModeGroup, Name = FullState)]
    [TemplateVisualState(GroupName = DisplayModeGroup, Name = CompactDetailState)]
    [TemplateVisualState(GroupName = DisplayModeGroup, Name = CompactMasterState)]
    [TemplateVisualState(GroupName = SelectionGroup, Name = HasNoSelectionState)]
    [TemplateVisualState(GroupName = SelectionGroup, Name = HasSelectionState)]
    public class MasterDetail : Control
    {
        private const string DisplayModeGroup = "DisplayModes";
        private const string SelectionGroup = "Selection";
        private const string HasNoSelectionState = "HasNoSelection";
        private const string HasSelectionState = "HasSelection";
        private const string FullState = "Full";
        private const string CompactMasterState = "CompactMaster";
        private const string CompactDetailState = "CompactDetail";

        private Frame _frame;

        /// <summary>
        ///     The master property
        /// </summary>
        public static readonly DependencyProperty MasterProperty = DependencyProperty.Register(
            nameof(Master), typeof(UIElement), typeof(MasterDetail), new PropertyMetadata(default(UIElement)));

        /// <summary>
        ///     The detail property
        /// </summary>
        public static readonly DependencyProperty DetailProperty = DependencyProperty.Register(
            nameof(Detail), typeof(object), typeof(MasterDetail), new PropertyMetadata(default(object), (o, args) => (o as MasterDetail)?.Update()));

        /// <summary>
        ///     The detail template property
        /// </summary>
        public static readonly DependencyProperty DetailTemplateProperty = DependencyProperty.Register(
            nameof(DetailTemplate), typeof(DataTemplate), typeof(MasterDetail), new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        ///     The detail template selector property
        /// </summary>
        public static readonly DependencyProperty DetailTemplateSelectorProperty = DependencyProperty.Register(
            nameof(DetailTemplateSelector), typeof(DataTemplateSelector), typeof(MasterDetail), new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        ///     The display mode property
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register(
            nameof(DisplayMode), typeof(MasterDetailDisplayMode), typeof(MasterDetail), new PropertyMetadata(default(MasterDetailDisplayMode), (o, args) => (o as MasterDetail)?.Update()));

        /// <summary>
        ///     The display visible property
        /// </summary>
        public static readonly DependencyProperty DisplayVisibleProperty = DependencyProperty.Register(
            nameof(DisplayVisible), typeof(MasterDetailDisplayVisible), typeof(MasterDetail), new PropertyMetadata(default(MasterDetailDisplayVisible), OnDisplayVisibleChanged));

        /// <summary>
        ///     The no selection view property
        /// </summary>
        public static readonly DependencyProperty NoSelectionViewProperty = DependencyProperty.Register(
            nameof(NoSelectionView), typeof(UIElement), typeof(MasterDetail), new PropertyMetadata(default(UIElement)));

        /// <summary>
        ///     The master width property
        /// </summary>
        public static readonly DependencyProperty MasterWidthProperty = DependencyProperty.Register(
            nameof(MasterWidth), typeof(double), typeof(MasterDetail), new PropertyMetadata(default(double)));

        /// <summary>
        ///     The display system back button on detail property
        /// </summary>
        public static readonly DependencyProperty DisplaySystemBackButtonOnDetailProperty = DependencyProperty.Register(
            nameof(DisplaySystemBackButtonOnDetail), typeof(bool), typeof(MasterDetail), new PropertyMetadata(default(bool)));

        /// <summary>
        /// The detail transitions property
        /// </summary>
        public static readonly DependencyProperty DetailTransitionsProperty = DependencyProperty.Register(
            nameof(DetailTransitions), typeof(TransitionCollection), typeof(MasterDetail), new PropertyMetadata(default(TransitionCollection)));

        /// <summary>
        /// The minimum width for full display mode property
        /// </summary>
        public static readonly DependencyProperty MinWidthForFullDisplayModeProperty = DependencyProperty.Register(
            nameof(MinWidthForFullDisplayMode), typeof(double), typeof(MasterDetail), new PropertyMetadata(default(double), OnMinWidthForFullDisplayModeChanged));

        private static void OnMinWidthForFullDisplayModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is double))
            {
                return;
            }

            var width = (double)e.NewValue;
            (sender as MasterDetail)?.SetDisplayMode(width);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDetail" /> class.
        /// </summary>
        public MasterDetail()
        {
            DefaultStyleKey = typeof(MasterDetail);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// Gets or sets the minimum width for full display mode.
        /// </summary>
        /// <value>
        /// The minimum width for full display mode.
        /// </value>
        public double MinWidthForFullDisplayMode
        {
            get { return (double)GetValue(MinWidthForFullDisplayModeProperty); }
            set { SetValue(MinWidthForFullDisplayModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the detail transitions.
        /// </summary>
        /// <value>
        /// The detail transitions.
        /// </value>
        public TransitionCollection DetailTransitions
        {
            get { return (TransitionCollection)GetValue(DetailTransitionsProperty); }
            set { SetValue(DetailTransitionsProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [display system back button on detail].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [display system back button on detail]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplaySystemBackButtonOnDetail
        {
            get { return (bool)GetValue(DisplaySystemBackButtonOnDetailProperty); }
            set { SetValue(DisplaySystemBackButtonOnDetailProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the width of the master.
        /// </summary>
        /// <value>
        ///     The width of the master.
        /// </value>
        public double MasterWidth
        {
            get { return (double)GetValue(MasterWidthProperty); }
            set { SetValue(MasterWidthProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the no selection view.
        /// </summary>
        /// <value>
        ///     The no selection view.
        /// </value>
        public UIElement NoSelectionView
        {
            get { return (UIElement)GetValue(NoSelectionViewProperty); }
            set { SetValue(NoSelectionViewProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the master.
        /// </summary>
        /// <value>
        ///     The master.
        /// </value>
        public UIElement Master
        {
            get { return (UIElement)GetValue(MasterProperty); }
            set { SetValue(MasterProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the detail.
        /// </summary>
        /// <value>
        ///     The detail.
        /// </value>
        public object Detail
        {
            get { return GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the detail template.
        /// </summary>
        /// <value>
        ///     The detail template.
        /// </value>
        public DataTemplate DetailTemplate
        {
            get { return (DataTemplate)GetValue(DetailTemplateProperty); }
            set { SetValue(DetailTemplateProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the detail template selector.
        /// </summary>
        /// <value>
        ///     The detail template selector.
        /// </value>
        public DataTemplateSelector DetailTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DetailTemplateSelectorProperty); }
            set { SetValue(DetailTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the display mode.
        /// </summary>
        /// <value>
        ///     The display mode.
        /// </value>
        public MasterDetailDisplayMode DisplayMode
        {
            get { return (MasterDetailDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the display visible.
        /// </summary>
        /// <value>
        ///     The display visible.
        /// </value>
        public MasterDetailDisplayVisible DisplayVisible
        {
            get { return (MasterDetailDisplayVisible)GetValue(DisplayVisibleProperty); }
            set { SetValue(DisplayVisibleProperty, value); }
        }

        private static void OnDisplayVisibleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((MasterDetailDisplayVisible)e.OldValue != (MasterDetailDisplayVisible)e.NewValue)
            {
                (sender as MasterDetail)?.SendDisplayVisibleChangedEvent();
            }
        }

        /// <summary>
        ///     Occurs when [display visible changed].
        /// </summary>
        public event EventHandler<DisplayVisibleArgs> DisplayVisibleChanged;

        /// <summary>
        ///     Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In
        ///     simplest terms, this means the method is called just before a UI element displays in your app. Override this method
        ///     to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetDisplayMode(Window.Current.Bounds.Width);

            Update();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;

            var frame = GetFrame();
            if (frame != null)
            {
                frame.Navigating -= FrameOnNavigating;
            }

            Window.Current.SizeChanged -= CurrentOnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            var frame = GetFrame();
            if (frame != null)
            {
                frame.Navigating += FrameOnNavigating;
            }

            Window.Current.SizeChanged -= CurrentOnSizeChanged;
            Window.Current.SizeChanged += CurrentOnSizeChanged;
        }

        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var width = e.Size.Width;
            SetDisplayMode(width);
        }

        private void SetDisplayMode(double width)
        {
            DisplayMode = width <= MinWidthForFullDisplayMode ? MasterDetailDisplayMode.Compact : MasterDetailDisplayMode.Full;
        }

        private Frame GetFrame()
        {
            return _frame ?? (_frame = this.FindVisualAscendant<Frame>());
        }

        private void FrameOnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var isHandled = HandleBackButton(justCheck: true);
                e.Cancel = isHandled;
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var isHandled = HandleBackButton();
            e.Handled = isHandled;
        }

        private bool HandleBackButton(bool justCheck = false)
        {
            var cancelBackButton = false;
            switch (DisplayMode)
            {
                case MasterDetailDisplayMode.Full:
                    break;

                case MasterDetailDisplayMode.Compact:
                    var isDetail = Detail != null;
                    cancelBackButton = isDetail;

                    if (!justCheck)
                    {
                        VisualStateManager.GoToState(this, CompactMasterState, true);
                        ShowSystemBackButton();

                        Detail = null;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return cancelBackButton;
        }

        private void ShowSystemBackButton()
        {
            if (DisplaySystemBackButtonOnDetail)
            {
                var backButtonShouldBeShown = ((DisplayMode == MasterDetailDisplayMode.Compact) && (Detail != null)) || (_frame?.CanGoBack ?? false);
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = backButtonShouldBeShown ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void Update()
        {
            switch (DisplayMode)
            {
                case MasterDetailDisplayMode.Full:
                    VisualStateManager.GoToState(this, FullState, true);
                    DisplayVisible = MasterDetailDisplayVisible.Both;
                    break;
                case MasterDetailDisplayMode.Compact:
                    var isDetail = Detail != null;
                    var state = isDetail ? CompactDetailState : CompactMasterState;

                    VisualStateManager.GoToState(this, state, true);
                    DisplayVisible = isDetail ? MasterDetailDisplayVisible.Detail : MasterDetailDisplayVisible.Master;
                    ShowSystemBackButton();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var selectionState = (Detail != null) || (DisplayMode == MasterDetailDisplayMode.Compact) ? HasSelectionState : HasNoSelectionState;
            VisualStateManager.GoToState(this, selectionState, true);
        }

        private void SendDisplayVisibleChangedEvent()
        {
            var eventHandler = DisplayVisibleChanged;
            eventHandler?.Invoke(this, new DisplayVisibleArgs(DisplayVisible));
        }
    }
}