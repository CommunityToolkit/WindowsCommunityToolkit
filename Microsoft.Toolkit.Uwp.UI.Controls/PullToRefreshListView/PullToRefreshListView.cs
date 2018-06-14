// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Extension of ListView that allows "Pull To Refresh" on touch devices
    /// </summary>
    [TemplatePart(Name = PartRoot, Type = typeof(Border))]
    [TemplatePart(Name = PartScroller, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PartContentTransform, Type = typeof(CompositeTransform))]
    [TemplatePart(Name = PartScrollerContent, Type = typeof(Grid))]
    [TemplatePart(Name = PartRefreshIndicatorBorder, Type = typeof(Border))]
    [TemplatePart(Name = PartIndicatorTransform, Type = typeof(CompositeTransform))]
    [TemplatePart(Name = PartDefaultIndicatorContent, Type = typeof(TextBlock))]
    [TemplatePart(Name = PullAndReleaseIndicatorContent, Type = typeof(ContentPresenter))]
    [Obsolete("The PullToRefreshListView will be removed in a future major release. Please use the RefreshContainer control available in the 1803 version of Windows")]
    public class PullToRefreshListView : ListView
    {
        /// <summary>
        /// Identifies the <see cref="OverscrollLimit"/> property.
        /// </summary>
        public static readonly DependencyProperty OverscrollLimitProperty =
            DependencyProperty.Register(nameof(OverscrollLimit), typeof(double), typeof(PullToRefreshListView), new PropertyMetadata(0.4, OverscrollLimitPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="PullThreshold"/> property.
        /// </summary>
        public static readonly DependencyProperty PullThresholdProperty =
            DependencyProperty.Register(nameof(PullThreshold), typeof(double), typeof(PullToRefreshListView), new PropertyMetadata(100.0));

        /// <summary>
        /// Identifies the <see cref="RefreshCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register(nameof(RefreshCommand), typeof(ICommand), typeof(PullToRefreshListView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RefreshIntentCanceledCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty RefreshIntentCanceledCommandProperty =
            DependencyProperty.Register(nameof(RefreshIntentCanceledCommand), typeof(ICommand), typeof(PullToRefreshListView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RefreshIndicatorContent"/> property.
        /// </summary>
        public static readonly DependencyProperty RefreshIndicatorContentProperty =
            DependencyProperty.Register(nameof(RefreshIndicatorContent), typeof(object), typeof(PullToRefreshListView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PullToRefreshLabel"/> property.
        /// </summary>
        public static readonly DependencyProperty PullToRefreshLabelProperty =
            DependencyProperty.Register(nameof(PullToRefreshLabel), typeof(object), typeof(PullToRefreshListView), new PropertyMetadata("Pull To Refresh", OnPullToRefreshLabelChanged));

        /// <summary>
        /// Identifies the <see cref="ReleaseToRefreshLabel"/> property.
        /// </summary>
        public static readonly DependencyProperty ReleaseToRefreshLabelProperty =
            DependencyProperty.Register(nameof(ReleaseToRefreshLabel), typeof(object), typeof(PullToRefreshListView), new PropertyMetadata("Release to Refresh", OnReleaseToRefreshLabelChanged));

        /// <summary>
        /// Identifies the <see cref="PullToRefreshContent"/> property.
        /// </summary>
        public static readonly DependencyProperty PullToRefreshContentProperty =
            DependencyProperty.Register(nameof(PullToRefreshContent), typeof(object), typeof(PullToRefreshListView), new PropertyMetadata("Pull To Refresh"));

        /// <summary>
        /// Identifies the <see cref="ReleaseToRefreshContent"/> property.
        /// </summary>
        public static readonly DependencyProperty ReleaseToRefreshContentProperty =
            DependencyProperty.Register(nameof(ReleaseToRefreshContent), typeof(object), typeof(PullToRefreshListView), new PropertyMetadata("Release to Refresh"));

        /// <summary>
        /// IsPullToRefreshWithMouseEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsPullToRefreshWithMouseEnabledProperty =
            DependencyProperty.Register(nameof(IsPullToRefreshWithMouseEnabled), typeof(bool), typeof(PullToRefreshListView), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="UseRefreshContainerWhenPossible"/> dependency property
        /// </summary>
        public static readonly DependencyProperty UseRefreshContainerWhenPossibleProperty =
            DependencyProperty.Register(nameof(UseRefreshContainerWhenPossible), typeof(bool), typeof(PullToRefreshListView), new PropertyMetadata(false, OnUseRefreshContainerWhenPossibleChanged));

        /// <summary>
        /// Gets a value indicating whether <see cref="RefreshContainer"/> is supported
        /// </summary>
        public static bool IsRefreshContainerSupported { get; } = ApiInformation.IsTypePresent("Windows.UI.Xaml.Controls.RefreshContainer");

        private const string PartRoot = "Root";
        private const string PartScroller = "ScrollViewer";
        private const string PartContentTransform = "ContentTransform";
        private const string PartScrollerContent = "ItemsPresenter";
        private const string PartRefreshIndicatorBorder = "RefreshIndicator";
        private const string PartIndicatorTransform = "RefreshIndicatorTransform";
        private const string PartDefaultIndicatorContent = "DefaultIndicatorContent";
        private const string PullAndReleaseIndicatorContent = "PullAndReleaseIndicatorContent";
        private const string PartRefreshContainer = "RefreshContainer";

        private Border _root;
        private Border _refreshIndicatorBorder;
        private CompositeTransform _refreshIndicatorTransform;
        private ScrollViewer _scroller;
        private CompositeTransform _contentTransform;
        private CompositeTransform _headerTransform;
        private CompositeTransform _footerTransform;
        private ItemsPresenter _scrollerContent;
        private TextBlock _defaultIndicatorContent;
        private ContentPresenter _pullAndReleaseIndicatorContent;
        private ScrollBar _scrollerVerticalScrollBar;
        private double _lastOffset = 0.0;
        private double _pullDistance = 0.0;
        private DateTime _lastRefreshActivation = default(DateTime);
        private bool _refreshActivated = false;
        private bool _refreshIntentCanceled = false;
        private double _overscrollMultiplier;
        private bool _isManipulatingWithMouse;
        private double _startingVerticalOffset;
        private ControlTemplate _previousTemplateUsed;
        private RefreshContainer _refreshContainer;

        private bool UsingRefreshContainer => IsRefreshContainerSupported && UseRefreshContainerWhenPossible;

        /// <summary>
        /// Gets or sets a value indicating whether the HamburgerMenu should use the NavigationView when possible (Fall Creators Update and above)
        /// When set to true and the device supports NavigationView, the HamburgerMenu will use a template based on NavigationView
        /// </summary>
        public bool UseRefreshContainerWhenPossible
        {
            get { return (bool)GetValue(UseRefreshContainerWhenPossibleProperty); }
            set { SetValue(UseRefreshContainerWhenPossibleProperty, value); }
        }

        /// <summary>
        /// Occurs when the user has requested content to be refreshed
        /// </summary>
        public event EventHandler RefreshRequested;

        /// <summary>
        /// Occurs when the user has cancels an intent for the content to be refreshed
        /// </summary>
        public event EventHandler RefreshIntentCanceled;

        /// <summary>
        /// Occurs when listview overscroll distance is changed
        /// </summary>
        public event EventHandler<RefreshProgressEventArgs> PullProgressChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="PullToRefreshListView"/> class.
        /// </summary>
        public PullToRefreshListView()
        {
            DefaultStyleKey = typeof(PullToRefreshListView);
            SizeChanged += RefreshableListView_SizeChanged;
        }

        /// <summary>
        /// Handler for SizeChanged event, handles cliping
        /// </summary>
        private void RefreshableListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding
        /// layout pass) call <see cref="OnApplyTemplate"/>. In simplest terms, this means the method
        /// is called just before a UI element displays in an application. Override this
        /// method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_scroller != null)
            {
                _scroller.Loaded -= Scroller_Loaded;
                _scroller.DirectManipulationCompleted -= Scroller_DirectManipulationCompleted;
                _scroller.DirectManipulationStarted -= Scroller_DirectManipulationStarted;
            }

            if (_refreshIndicatorBorder != null)
            {
                _refreshIndicatorBorder.SizeChanged -= RefreshIndicatorBorder_SizeChanged;
            }

            if (_root != null)
            {
                _root.ManipulationStarted -= Scroller_ManipulationStarted;
                _root.ManipulationCompleted -= Scroller_ManipulationCompleted;
            }

            _root = null;
            _refreshIndicatorBorder = null;
            _refreshIndicatorTransform = null;
            _scroller = null;
            _contentTransform = null;
            _headerTransform = null;
            _footerTransform = null;
            _scrollerContent = null;
            _defaultIndicatorContent = null;
            _pullAndReleaseIndicatorContent = null;
            _scrollerVerticalScrollBar = null;

            if (UsingRefreshContainer)
            {
                OnApplyRefreshContainerTemplate();
            }

            _root = GetTemplateChild(PartRoot) as Border;
            _scroller = GetTemplateChild(PartScroller) as ScrollViewer;
            _scrollerContent = GetTemplateChild(PartScrollerContent) as ItemsPresenter;
            _refreshIndicatorBorder = GetTemplateChild(PartRefreshIndicatorBorder) as Border;
            _refreshIndicatorTransform = GetTemplateChild(PartIndicatorTransform) as CompositeTransform;
            _defaultIndicatorContent = GetTemplateChild(PartDefaultIndicatorContent) as TextBlock;
            _pullAndReleaseIndicatorContent = GetTemplateChild(PullAndReleaseIndicatorContent) as ContentPresenter;

            if (_root != null &&
                _scroller != null &&
                _scrollerContent != null &&
                _refreshIndicatorBorder != null &&
                _refreshIndicatorTransform != null &&
                (_defaultIndicatorContent != null || _pullAndReleaseIndicatorContent != null))
            {
                _scroller.Loaded += Scroller_Loaded;

                SetupMouseMode();

                _scroller.DirectManipulationCompleted += Scroller_DirectManipulationCompleted;
                _scroller.DirectManipulationStarted += Scroller_DirectManipulationStarted;

                if (_defaultIndicatorContent != null)
                {
                    _defaultIndicatorContent.Visibility = RefreshIndicatorContent == null ? Visibility.Visible : Visibility.Collapsed;
                }

                if (_pullAndReleaseIndicatorContent != null)
                {
                    _pullAndReleaseIndicatorContent.Visibility = RefreshIndicatorContent == null ? Visibility.Visible : Visibility.Collapsed;
                }

                _refreshIndicatorBorder.SizeChanged += RefreshIndicatorBorder_SizeChanged;

                _overscrollMultiplier = OverscrollLimit * 8;
            }

            base.OnApplyTemplate();
        }

        private void OnApplyRefreshContainerTemplate()
        {
            if (_refreshContainer != null)
            {
                _refreshContainer.RefreshRequested -= RefreshContainer_RefreshRequested;
            }

            _refreshContainer = GetTemplateChild(PartRefreshContainer) as RefreshContainer;

            if (_refreshContainer != null)
            {
                _refreshContainer.RefreshRequested += RefreshContainer_RefreshRequested;
            }
        }

        private void Scroller_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            // Other input are already managed by the scroll viewer
            if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse
                && IsPullToRefreshWithMouseEnabled)
            {
                if (_scroller.VerticalOffset < 1)
                {
                    DisplayPullToRefreshContent();
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                    _isManipulatingWithMouse = true;
                }

                _startingVerticalOffset = _scroller.VerticalOffset;
                _root.ManipulationDelta -= Scroller_ManipulationDelta;
                _root.ManipulationDelta += Scroller_ManipulationDelta;
            }
        }

        private void Scroller_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _root.ManipulationDelta -= Scroller_ManipulationDelta;

            if (!IsPullToRefreshWithMouseEnabled)
            {
                return;
            }

            OnManipulationCompleted();
        }

        private void Scroller_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            if (!IsPullToRefreshWithMouseEnabled || _contentTransform == null)
            {
                return;
            }

            if (e.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                return;
            }

            if (e.Cumulative.Translation.Y <= 0 || _scroller.VerticalOffset >= 1)
            {
                _scroller.ChangeView(_scroller.HorizontalOffset, _scroller.VerticalOffset - e.Delta.Translation.Y, 1);
                return;
            }

            if (_startingVerticalOffset >= 1)
            {
                return;
            }

            // content is not "moved" automagically by the scrollviewer in this case
            // so we need to apply our own transformation.
            // and to do so we use a little Sin Easing.

            // how much "drag" to go to the max translation
            var mouseMaxDragDistance = 100;

            // make it harder to drag (life is not easy)
            double translationToUse = e.Cumulative.Translation.Y / 3;
            var deltaCumulative = Math.Min(translationToUse, mouseMaxDragDistance) / mouseMaxDragDistance;

            // let's do some quartic ease-out
            double f = deltaCumulative - 1;
            var easing = 1 + (f * f * f * (1 - deltaCumulative));

            var maxTranslation = 150;
            _contentTransform.TranslateY = easing * maxTranslation;

            if (_headerTransform != null)
            {
                _headerTransform.TranslateY = _contentTransform.TranslateY;
            }

            if (_footerTransform != null)
            {
                _footerTransform.TranslateY = _contentTransform.TranslateY;
            }
        }

        private void RefreshIndicatorBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _refreshIndicatorTransform.TranslateY = -_refreshIndicatorBorder.ActualHeight;
        }

        private void Scroller_DirectManipulationStarted(object sender, object e)
        {
            // sometimes the value gets stuck at 0.something, so checking if less than 1
            if (_scroller.VerticalOffset < 1)
            {
                OnManipulationCompleted();
                DisplayPullToRefreshContent();
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }

        /// <summary>
        /// Display the pull to refresh content
        /// </summary>
        private void DisplayPullToRefreshContent()
        {
            if (RefreshIndicatorContent == null)
            {
                if (_defaultIndicatorContent != null)
                {
                    _defaultIndicatorContent.Text = PullToRefreshLabel;
                }

                if (_pullAndReleaseIndicatorContent != null)
                {
                    _pullAndReleaseIndicatorContent.Content = PullToRefreshContent;
                }
            }
        }

        private void Scroller_DirectManipulationCompleted(object sender, object e)
        {
            OnManipulationCompleted();
            _root.ManipulationMode = ManipulationModes.System;
        }

        /// <summary>
        /// Method called at the end of manipulation to clean up everything
        /// </summary>
        private void OnManipulationCompleted()
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            _refreshIndicatorTransform.TranslateY = -_refreshIndicatorBorder.ActualHeight;
            if (_contentTransform != null)
            {
                _contentTransform.TranslateY = 0;

                if (_headerTransform != null)
                {
                    _headerTransform.TranslateY = 0;
                }

                if (_footerTransform != null)
                {
                    _footerTransform.TranslateY = 0;
                }
            }

            if (_refreshActivated)
            {
                RefreshRequested?.Invoke(this, EventArgs.Empty);
                if (RefreshCommand != null && RefreshCommand.CanExecute(null))
                {
                    RefreshCommand.Execute(null);
                }
            }
            else if (_refreshIntentCanceled)
            {
                RefreshIntentCanceled?.Invoke(this, EventArgs.Empty);
                if (RefreshIntentCanceledCommand != null && RefreshIntentCanceledCommand.CanExecute(null))
                {
                    RefreshIntentCanceledCommand.Execute(null);
                }
            }

            _lastOffset = 0;
            _pullDistance = 0;
            _refreshActivated = false;
            _refreshIntentCanceled = false;
            _lastRefreshActivation = default(DateTime);
            _isManipulatingWithMouse = false;

            PullProgressChanged?.Invoke(this, new RefreshProgressEventArgs { PullProgress = 0 });
            _pullAndReleaseIndicatorContent.Content = null;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            // if started navigating down, cancel the refresh
            if (_scroller.VerticalOffset > 1)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                _refreshIndicatorTransform.TranslateY = -_refreshIndicatorBorder.ActualHeight;
                if (_contentTransform != null)
                {
                    _contentTransform.TranslateY = 0;

                    if (_headerTransform != null)
                    {
                        _headerTransform.TranslateY = 0;
                    }

                    if (_footerTransform != null)
                    {
                        _footerTransform.TranslateY = 0;
                    }
                }

                _refreshActivated = false;
                _lastRefreshActivation = default(DateTime);

                PullProgressChanged?.Invoke(this, new RefreshProgressEventArgs { PullProgress = 0 });
                _isManipulatingWithMouse = false;

                return;
            }

            if (_contentTransform == null)
            {
                var headerContent = VisualTreeHelper.GetChild(_scrollerContent, 0) as UIElement;
                var itemsPanel = VisualTreeHelper.GetChild(_scrollerContent, 1) as UIElement;
                var footerContent = VisualTreeHelper.GetChild(_scrollerContent, 2) as UIElement;

                if (_headerTransform == null && VisualTreeHelper.GetChildrenCount(headerContent) > 0)
                {
                    if (headerContent != null)
                    {
                        _headerTransform = new CompositeTransform();
                        headerContent.RenderTransform = _headerTransform;
                    }
                }

                if (_footerTransform == null && VisualTreeHelper.GetChildrenCount(footerContent) > 0)
                {
                    if (footerContent != null)
                    {
                        _footerTransform = new CompositeTransform();
                        footerContent.RenderTransform = _footerTransform;
                    }
                }

                if (itemsPanel == null)
                {
                    return;
                }

                _contentTransform = new CompositeTransform();
                itemsPanel.RenderTransform = _contentTransform;
            }

            Rect elementBounds = _scrollerContent.TransformToVisual(_root).TransformBounds(default(Rect));

            // content is not "moved" automagically by the scrollviewer in this case
            // so we apply our own transformation too and need to take it in account.
            if (_isManipulatingWithMouse)
            {
                elementBounds = _contentTransform.TransformBounds(elementBounds);
            }

            var offset = elementBounds.Y;
            var delta = offset - _lastOffset;
            _lastOffset = offset;

            _pullDistance += delta * _overscrollMultiplier;

            if (_isManipulatingWithMouse)
            {
                _pullDistance = 2 * offset;
            }

            if (_pullDistance > 0)
            {
                if (!_isManipulatingWithMouse)
                {
                    _contentTransform.TranslateY = _pullDistance - offset;

                    if (_headerTransform != null)
                    {
                        _headerTransform.TranslateY = _contentTransform.TranslateY;
                    }

                    if (_footerTransform != null)
                    {
                        _footerTransform.TranslateY = _contentTransform.TranslateY;
                    }
                }

                if (_isManipulatingWithMouse)
                {
                    _refreshIndicatorTransform.TranslateY = _pullDistance - offset
                                                        - _refreshIndicatorBorder.ActualHeight;
                }
                else
                {
                    _refreshIndicatorTransform.TranslateY = _pullDistance
                                                        - _refreshIndicatorBorder.ActualHeight;
                }
            }
            else
            {
                if (!_isManipulatingWithMouse)
                {
                    _contentTransform.TranslateY = 0;

                    if (_headerTransform != null)
                    {
                        _headerTransform.TranslateY = _contentTransform.TranslateY;
                    }

                    if (_footerTransform != null)
                    {
                        _footerTransform.TranslateY = _contentTransform.TranslateY;
                    }
                }

                _refreshIndicatorTransform.TranslateY = -_refreshIndicatorBorder.ActualHeight;
            }

            double pullProgress;
            if (_pullDistance >= PullThreshold)
            {
                _lastRefreshActivation = DateTime.Now;
                _refreshActivated = true;
                _refreshIntentCanceled = false;
                pullProgress = 1.0;
                if (RefreshIndicatorContent == null)
                {
                    if (_defaultIndicatorContent != null)
                    {
                        _defaultIndicatorContent.Text = ReleaseToRefreshLabel;
                    }

                    if (_pullAndReleaseIndicatorContent != null)
                    {
                        _pullAndReleaseIndicatorContent.Content = ReleaseToRefreshContent;
                    }
                }
            }
            else if (_lastRefreshActivation != DateTime.MinValue)
            {
                TimeSpan timeSinceActivated = DateTime.Now - _lastRefreshActivation;

                // if more then a second since activation, deactivate
                if (timeSinceActivated.TotalMilliseconds > 1000)
                {
                    _refreshIntentCanceled |= _refreshActivated;
                    _refreshActivated = false;
                    _lastRefreshActivation = default(DateTime);
                    pullProgress = _pullDistance / PullThreshold;
                    if (RefreshIndicatorContent == null)
                    {
                        if (_defaultIndicatorContent != null)
                        {
                            _defaultIndicatorContent.Text = PullToRefreshLabel;
                        }

                        if (_pullAndReleaseIndicatorContent != null)
                        {
                            _pullAndReleaseIndicatorContent.Content = PullToRefreshContent;
                        }
                    }
                }
                else
                {
                    pullProgress = 1.0;
                    _refreshIntentCanceled |= _refreshActivated;
                }
            }
            else
            {
                pullProgress = _pullDistance / PullThreshold;
                _refreshIntentCanceled |= _refreshActivated;
            }

            PullProgressChanged?.Invoke(this, new RefreshProgressEventArgs { PullProgress = pullProgress });
        }

        private void Scroller_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollerVerticalScrollBar = _scroller.FindDescendantByName("VerticalScrollBar") as ScrollBar;
            _scrollerVerticalScrollBar.PointerEntered += ScrollerVerticalScrollBar_PointerEntered;
            _scrollerVerticalScrollBar.PointerExited += ScrollerVerticalScrollBar_PointerExited;
        }

        private void Scroller_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _root.ManipulationMode = ManipulationModes.System;
        }

        private void Scroller_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsPullToRefreshWithMouseEnabled && e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                _root.ManipulationMode = ManipulationModes.TranslateY;
            }
        }

        private void ScrollerVerticalScrollBar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _root.ManipulationMode = ManipulationModes.System;
            _root.ManipulationStarted -= Scroller_ManipulationStarted;
            _root.ManipulationCompleted -= Scroller_ManipulationCompleted;
        }

        private void ScrollerVerticalScrollBar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (IsPullToRefreshWithMouseEnabled)
            {
                _root.ManipulationStarted -= Scroller_ManipulationStarted;
                _root.ManipulationCompleted -= Scroller_ManipulationCompleted;
                _root.ManipulationStarted += Scroller_ManipulationStarted;
                _root.ManipulationCompleted += Scroller_ManipulationCompleted;
            }
        }

        private void RefreshContainer_RefreshRequested(object sender, RefreshRequestedEventArgs args)
        {
            using (var deferral = args.GetDeferral())
            {
                RefreshRequested?.Invoke(this, EventArgs.Empty);
                if (RefreshCommand != null && RefreshCommand.CanExecute(null))
                {
                    RefreshCommand.Execute(null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Overscroll Limit. Value between 0 and 1 where 1 is the height of the control. Default is 0.3
        /// </summary>
        public double OverscrollLimit
        {
            get { return (double)GetValue(OverscrollLimitProperty); }
            set { SetValue(OverscrollLimitProperty, value); }
        }

        private static void OverscrollLimitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double value = (double)e.NewValue;
            PullToRefreshListView view = d as PullToRefreshListView;

            if (value >= 0 && value <= 1)
            {
                view._overscrollMultiplier = value * 8;
            }
            else
            {
                throw new IndexOutOfRangeException("OverscrollCoefficient has to be a double value between 0 and 1 inclusive.");
            }
        }

        private static void OnPullToRefreshLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(PullToRefreshContentProperty, e.NewValue);
        }

        private static void OnReleaseToRefreshLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(ReleaseToRefreshLabelProperty, e.NewValue);
        }

        private static void OnUseRefreshContainerWhenPossibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var list = d as PullToRefreshListView;
            if (list == null)
            {
                return;
            }

            if (list.UseRefreshContainerWhenPossible && IsRefreshContainerSupported)
            {
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new System.Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls/PullToRefreshListView/PullToRefreshListViewRefreshContainerTemplate.xaml");
                list._previousTemplateUsed = list.Template;
                list.Template = dict["PullToRefreshListViewRefreshContainerTemplate"] as ControlTemplate;
            }
            else if (!list.UseRefreshContainerWhenPossible &&
                     e.OldValue is bool oldValue &&
                     oldValue &&
                     list._previousTemplateUsed != null)
            {
                list.Template = list._previousTemplateUsed;
            }
        }

        /// <summary>
        /// Gets or sets the PullThreshold in pixels for when Refresh should be Requested. Default is 100
        /// </summary>
        public double PullThreshold
        {
            get { return (double)GetValue(PullThresholdProperty); }
            set { SetValue(PullThresholdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Command that will be invoked when Refresh is requested
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Command that will be invoked when a refresh intent is cancled
        /// </summary>
        public ICommand RefreshIntentCanceledCommand
        {
            get { return (ICommand)GetValue(RefreshIntentCanceledCommandProperty); }
            set { SetValue(RefreshIntentCanceledCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Content of the Refresh Indicator
        /// </summary>
        public object RefreshIndicatorContent
        {
            get
            {
                return (object)GetValue(RefreshIndicatorContentProperty);
            }

            set
            {
                if (_defaultIndicatorContent != null && _pullAndReleaseIndicatorContent != null)
                {
                    _defaultIndicatorContent.Visibility = Visibility.Collapsed;
                }
                else if (_defaultIndicatorContent != null)
                {
                    _defaultIndicatorContent.Visibility = value == null ? Visibility.Visible : Visibility.Collapsed;
                }

                if (_pullAndReleaseIndicatorContent != null)
                {
                    _pullAndReleaseIndicatorContent.Visibility = value == null ? Visibility.Visible : Visibility.Collapsed;
                }

                SetValue(RefreshIndicatorContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label that will be shown when the user pulls down to refresh.
        /// Note: This label will only show up if <see cref="RefreshIndicatorContent" /> is null
        /// </summary>
        public string PullToRefreshLabel
        {
            get { return (string)GetValue(PullToRefreshLabelProperty); }
            set { SetValue(PullToRefreshLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label that will be shown when the user needs to release to refresh.
        /// Note: This label will only show up if <see cref="RefreshIndicatorContent" /> is null
        /// </summary>
        public string ReleaseToRefreshLabel
        {
            get { return (string)GetValue(ReleaseToRefreshLabelProperty); }
            set { SetValue(ReleaseToRefreshLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content that will be shown when the user pulls down to refresh.
        /// </summary>
        /// <remarks>
        /// This content will only show up if <see cref="RefreshIndicatorContent" /> is null
        /// </remarks>
        public object PullToRefreshContent
        {
            get { return (object)GetValue(PullToRefreshContentProperty); }
            set { SetValue(PullToRefreshContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content that will be shown when the user needs to release to refresh.
        /// </summary>
        /// <remarks>
        /// This content will only show up if <see cref="RefreshIndicatorContent" /> is null
        /// </remarks>
        public object ReleaseToRefreshContent
        {
            get { return (object)GetValue(ReleaseToRefreshContentProperty); }
            set { SetValue(ReleaseToRefreshContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether PullToRefresh is enabled with a mouse
        /// </summary>
        public bool IsPullToRefreshWithMouseEnabled
        {
            get
            {
                return (bool)GetValue(IsPullToRefreshWithMouseEnabledProperty);
            }

            set
            {
                SetValue(IsPullToRefreshWithMouseEnabledProperty, value);
                SetupMouseMode();
            }
        }

        private void SetupMouseMode()
        {
            if (_root != null && _scroller != null)
            {
                if (IsPullToRefreshWithMouseEnabled)
                {
                    _root.ManipulationStarted -= Scroller_ManipulationStarted;
                    _root.ManipulationCompleted -= Scroller_ManipulationCompleted;
                    _scroller.PointerMoved -= Scroller_PointerMoved;
                    _scroller.PointerExited -= Scroller_PointerExited;

                    _root.ManipulationStarted += Scroller_ManipulationStarted;
                    _root.ManipulationCompleted += Scroller_ManipulationCompleted;
                    _scroller.PointerMoved += Scroller_PointerMoved;
                    _scroller.PointerExited += Scroller_PointerExited;
                }
                else
                {
                    _root.ManipulationMode = ManipulationModes.System;
                    _root.ManipulationStarted -= Scroller_ManipulationStarted;
                    _root.ManipulationCompleted -= Scroller_ManipulationCompleted;
                    _scroller.PointerMoved -= Scroller_PointerMoved;
                    _scroller.PointerExited -= Scroller_PointerExited;
                }
            }
        }
    }
}
