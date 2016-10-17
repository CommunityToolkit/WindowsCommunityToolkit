using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class MasterDetailsView : ItemsControl
    {
        private ContentPresenter _presenter;
        private VisualStateGroup _stateGroup;
        private VisualState _narrowState;
        private VisualState _currentState;
        private SplitView _splitView;
        private Frame _frame;

        public MasterDetailsView()
        {
            DefaultStyleKey = typeof(MasterDetailsView);

            Loaded += OnLoaded;
            Unloaded += UnLoaded;
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <returns>The selected item. The default is null.</returns>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="SelectedItem"/> dependency property.</returns>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(MasterDetailsView),
            new PropertyMetadata(null, OnSelectedItemChanged));

        /// <summary>
        /// Gets or sets the DataTemplate used to display the details.
        /// </summary>
        public DataTemplate DetailsTemplate
        {
            get { return (DataTemplate)GetValue(DetailsTemplateProperty); }
            set { SetValue(DetailsTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DetailsTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="DetailsTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register(
            nameof(DetailsTemplate),
            typeof(DataTemplate),
            typeof(MasterDetailsView),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Brush to apply to the background of the list area of the control.
        /// </summary>
        /// <returns>The Brush to apply to the background of the list area of the control.</returns>
        public Brush ListPaneBackground
        {
            get { return (Brush)GetValue(ListPaneBackgroundProperty); }
            set { SetValue(ListPaneBackgroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ListPaneBackground"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="ListPaneBackground"/> dependency property.</returns>
        public static readonly DependencyProperty ListPaneBackgroundProperty = DependencyProperty.Register(
            nameof(ListPaneBackground),
            typeof(Brush),
            typeof(MasterDetailsView),
            new PropertyMetadata(null));

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _splitView = GetTemplateChild("MasterSplitView") as SplitView;
            if (_splitView != null)
            {
                _splitView.SizeChanged += OnSplitViewSizeChanged;
                _splitView.PaneClosing += OnMasterPaneClosing;
            }
            _presenter = GetTemplateChild("DetailsPresenter") as ContentPresenter;
        }

        /// <summary>
        /// Handles showing the back button in narrow mode and determining if the
        /// list pane should be open
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (MasterDetailsView)d;

            // quick hack to hide the content at first so the user doesn't have to
            // have some converter to hide text when selection is null
            panel._presenter.Visibility = Visibility.Visible;

            // Simple way to get the content to animate in
            panel._presenter.ContentTransitions.Clear();
            panel._presenter.Content = null;
            panel._presenter.ContentTransitions.Add(new EdgeUIThemeTransition { Edge = EdgeTransitionLocation.Right });
            panel._presenter.Content = panel.SelectedItem;

            panel.SetBackButtonVisibility(panel._stateGroup.CurrentState);
            if (panel._stateGroup.CurrentState == panel._narrowState)
            {
                panel._splitView.IsPaneOpen = panel.SelectedItem == null;
            }

        }

        // Have to wait to get the VisualStateGroup until the control has Loaded
        // If we try to get the VisualStateGroup in the OnApplyTemplate the 
        // CurrentStateChanged event does not fire properly
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            // The below event isn't firing properly. Using SizeChanged to handle 
            // if (_stateGroup != null)
            // {
            //    _stateGroup.CurrentStateChanged -= OnVisualStateChanged;
            // }
            _stateGroup = GetTemplateChild("WidthStates") as VisualStateGroup;

            // The below event isn't firing properly. Using SizeChanged to handle
            // _stateGroup.CurrentStateChanged += OnVisualStateChanged;

            _narrowState = GetTemplateChild("NarrowState") as VisualState;
            if (_splitView != null)
            {
                SetPaneLength(_splitView.ActualWidth);
            }
        }

        private void UnLoaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
        }

        /// <summary>
        /// Fires when the addaptive trigger changes state. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Handles showing the back button and if the list pane should be visible.
        /// Not working when not debugging. Resorting to SizeChanged event
        /// </remarks>
        private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            OnVisualStateChanged(e.NewState);
        }

        /// <summary>
        /// Fires when the size of the SplitView changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Handles setting the OpenLaneLength of the SplitView. When in narrow state the list should
        /// expand the entire width of the SplitView. We don't want this to be larger than the width
        /// because the content will then be wider than it should be.
        /// </remarks>
        private void OnSplitViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_stateGroup == null)
            {
                return;
            }

            // Work around for CurrentStateChanged not firing when not debugging
            if (_currentState != _stateGroup.CurrentState)
            {
                _currentState = _stateGroup.CurrentState;
                OnVisualStateChanged(_currentState);
            }

            SetPaneLength(e.NewSize.Width);
        }

        private void OnMasterPaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            // The pane should only close when we are in a narrow state and there is a selected item.
            // During Narrow state the DisplayMode is set to overlay to add animations. Side effect 
            // of using Overlay is the pane will close when resizing or focus is off of the pane.
            if (SelectedItem == null)
            {
                args.Cancel = true;
            }
        }

        /// <summary>
        /// Closed the details pane if we are in narrow state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnBackRequested(object sender, BackRequestedEventArgs args)
        {
            if (((_stateGroup.CurrentState == _narrowState) || (_stateGroup.CurrentState == null)) && (SelectedItem != null))
            {
                _splitView.IsPaneOpen = true;
                SelectedItem = null;
                args.Handled = true;
                SetBackButtonVisibility(_stateGroup.CurrentState);
            }
        }

        private void SetPaneLength(double width)
        {
            // Would prefer to use the adaptive trigger to assign this but binding to
            // the ActualWidth would not work
            if (_stateGroup.CurrentState == _narrowState)
            {
                _splitView.OpenPaneLength = width;
            }
        }

        private void OnVisualStateChanged(VisualState currentState)
        {
            if (currentState == _narrowState)
            {
                // The pane should only be open is narrow state if the SelectedItem is null
                _splitView.IsPaneOpen = SelectedItem == null;
            }

            SetBackButtonVisibility(currentState);
        }

        /// <summary>
        /// Sets the back button visibility based on the current visual state and selected item
        /// </summary>
        private void SetBackButtonVisibility(VisualState currentState)
        {
            if ((currentState == _narrowState) && (SelectedItem != null))
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Make sure we show the back button if the stack can navigate back
                var frame = GetFrame();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    ((frame != null) && frame.CanGoBack)
                        ? AppViewBackButtonVisibility.Visible
                        : AppViewBackButtonVisibility.Collapsed;
            }
        }

        private Frame GetFrame()
        {
            return _frame ?? (_frame = this.FindVisualAscendant<Frame>());
        }
    }
}
