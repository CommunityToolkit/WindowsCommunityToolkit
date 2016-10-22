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
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        /// Identifies the <see cref="RefreshIndicatorContent"/> property.
        /// </summary>
        public static readonly DependencyProperty RefreshIndicatorContentProperty =
            DependencyProperty.Register(nameof(RefreshIndicatorContent), typeof(object), typeof(PullToRefreshListView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PullToRefreshLabel"/> property.
        /// </summary>
        public static readonly DependencyProperty PullToRefreshLabelProperty =
            DependencyProperty.Register("PullToRefreshLabel", typeof(string), typeof(PullToRefreshListView), new PropertyMetadata("Pull To Refresh"));

        /// <summary>
        /// Identifies the <see cref="ReleaseToRefreshLabel"/> property.
        /// </summary>
        public static readonly DependencyProperty ReleaseToRefreshLabelProperty =
            DependencyProperty.Register("ReleaseToRefreshLabel", typeof(string), typeof(PullToRefreshListView), new PropertyMetadata("Release to Refresh"));

        private const string PartRoot = "Root";
        private const string PartScroller = "ScrollViewer";
        private const string PartContentTransform = "ContentTransform";
        private const string PartScrollerContent = "ItemsPresenter";
        private const string PartRefreshIndicatorBorder = "RefreshIndicator";
        private const string PartIndicatorTransform = "RefreshIndicatorTransform";
        private const string PartDefaultIndicatorContent = "DefaultIndicatorContent";

        private Border _root;
        private Border _refreshIndicatorBorder;
        private CompositeTransform _refreshIndicatorTransform;
        private ScrollViewer _scroller;
        private CompositeTransform _contentTransform;
        private ItemsPresenter _scrollerContent;
        private TextBlock _defaultIndicatorContent;
        private double _lastOffset = 0.0;
        private double _pullDistance = 0.0;
        private DateTime _lastRefreshActivation = default(DateTime);
        private bool _refreshActivated = false;
        private double _overscrollMultiplier;

        /// <summary>
        /// Occurs when the user has requested content to be refreshed
        /// </summary>
        public event EventHandler RefreshRequested;

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
                _scroller.DirectManipulationCompleted -= Scroller_DirectManipulationCompleted;
                _scroller.DirectManipulationStarted -= Scroller_DirectManipulationStarted;
            }

            if (_refreshIndicatorBorder != null)
            {
                _refreshIndicatorBorder.SizeChanged -= RefreshIndicatorBorder_SizeChanged;
            }

            _root = GetTemplateChild(PartRoot) as Border;
            _scroller = GetTemplateChild(PartScroller) as ScrollViewer;
            _scrollerContent = GetTemplateChild(PartScrollerContent) as ItemsPresenter;
            _refreshIndicatorBorder = GetTemplateChild(PartRefreshIndicatorBorder) as Border;
            _refreshIndicatorTransform = GetTemplateChild(PartIndicatorTransform) as CompositeTransform;
            _defaultIndicatorContent = GetTemplateChild(PartDefaultIndicatorContent) as TextBlock;

            if (_root != null &&
                _scroller != null &&
                _scrollerContent != null &&
                _refreshIndicatorBorder != null &&
                _refreshIndicatorTransform != null &&
                _defaultIndicatorContent != null)
            {
                _scroller.DirectManipulationCompleted += Scroller_DirectManipulationCompleted;
                _scroller.DirectManipulationStarted += Scroller_DirectManipulationStarted;

                _defaultIndicatorContent.Visibility = RefreshIndicatorContent == null ? Visibility.Visible : Visibility.Collapsed;

                _refreshIndicatorBorder.SizeChanged += RefreshIndicatorBorder_SizeChanged;

                _overscrollMultiplier = OverscrollLimit * 8;
            }

            base.OnApplyTemplate();
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
                if (RefreshIndicatorContent == null)
                {
                    _defaultIndicatorContent.Text = PullToRefreshLabel;
                }

                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }

        private void Scroller_DirectManipulationCompleted(object sender, object e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            _refreshIndicatorTransform.TranslateY = -_refreshIndicatorBorder.ActualHeight;
            if (_contentTransform != null)
            {
                _contentTransform.TranslateY = 0;
            }

            if (_refreshActivated)
            {
                RefreshRequested?.Invoke(this, new EventArgs());
                if (RefreshCommand != null && RefreshCommand.CanExecute(null))
                {
                    RefreshCommand.Execute(null);
                }
            }

            _refreshActivated = false;
            _lastRefreshActivation = default(DateTime);

            PullProgressChanged?.Invoke(this, new RefreshProgressEventArgs() { PullProgress = 0 });
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
                }

                _refreshActivated = false;
                _lastRefreshActivation = default(DateTime);

                PullProgressChanged?.Invoke(this, new RefreshProgressEventArgs() { PullProgress = 0 });

                return;
            }

            if (_contentTransform == null)
            {
                var itemScrollPanel = _scrollerContent.FindDescendant<Panel>();
                if (itemScrollPanel == null)
                {
                    return;
                }

                _contentTransform = new CompositeTransform();
                itemScrollPanel.RenderTransform = _contentTransform;
            }

            Rect elementBounds = _scrollerContent.TransformToVisual(_root).TransformBounds(default(Rect));

            var offset = elementBounds.Y;
            var delta = offset - _lastOffset;
            _lastOffset = offset;

            _pullDistance += delta * _overscrollMultiplier;

            if (_pullDistance > 0)
            {
                _contentTransform.TranslateY = _pullDistance - offset;
                _refreshIndicatorTransform.TranslateY = _pullDistance - offset - _refreshIndicatorBorder.ActualHeight;
            }
            else
            {
                _contentTransform.TranslateY = 0;
                _refreshIndicatorTransform.TranslateY = -_refreshIndicatorBorder.ActualHeight;
            }

            var pullProgress = 0.0;

            if (_pullDistance >= PullThreshold)
            {
                _lastRefreshActivation = DateTime.Now;
                _refreshActivated = true;
                pullProgress = 1.0;
                if (RefreshIndicatorContent == null)
                {
                    _defaultIndicatorContent.Text = ReleaseToRefreshLabel;
                }
            }
            else if (_lastRefreshActivation != DateTime.MinValue)
            {
                TimeSpan timeSinceActivated = DateTime.Now - _lastRefreshActivation;

                // if more then a second since activation, deactivate
                if (timeSinceActivated.TotalMilliseconds > 1000)
                {
                    _refreshActivated = false;
                    _lastRefreshActivation = default(DateTime);
                    pullProgress = _pullDistance / PullThreshold;
                    if (RefreshIndicatorContent == null)
                    {
                        _defaultIndicatorContent.Text = PullToRefreshLabel;
                    }
                }
                else
                {
                    pullProgress = 1.0;
                }
            }
            else
            {
                pullProgress = _pullDistance / PullThreshold;
            }

            PullProgressChanged?.Invoke(this, new RefreshProgressEventArgs { PullProgress = pullProgress });
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
                if (_defaultIndicatorContent != null)
                {
                    _defaultIndicatorContent.Visibility = value == null ? Visibility.Visible : Visibility.Collapsed;
                }

                SetValue(RefreshIndicatorContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label that will be shown when the user pulls down to refresh.
        /// Note: This label will only show up if <see cref="RefreshIndicatorContent" /> is null/>
        /// </summary>
        public string PullToRefreshLabel
        {
            get { return (string)GetValue(PullToRefreshLabelProperty); }
            set { SetValue(PullToRefreshLabelProperty, value); }
        }

        // <summary>
        // Gets or sets the label that will be shown when the user needs to release to refresh.
        // Note: This label will only show up if <see cref="RefreshIndicatorContent" /> is null/>
        // </summary>
        public string ReleaseToRefreshLabel
        {
            get { return (string)GetValue(ReleaseToRefreshLabelProperty); }
            set { SetValue(ReleaseToRefreshLabelProperty, value); }
        }
    }
}
