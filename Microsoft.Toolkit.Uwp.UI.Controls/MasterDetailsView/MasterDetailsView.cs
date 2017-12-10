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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Panel that allows for a Master/Details pattern.
    /// </summary>
    [TemplatePart(Name = PartDetailsPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartDetailsPanel, Type = typeof(FrameworkElement))]
    [TemplateVisualState(Name = NoSelectionNarrowState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = NoSelectionWideState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = NarrowState, GroupName = WidthStates)]
    [TemplateVisualState(Name = WideState, GroupName = WidthStates)]
    public partial class MasterDetailsView : ItemsControl
    {
        private const string PartDetailsPresenter = "DetailsPresenter";
        private const string PartDetailsPanel = "DetailsPanel";
        private const string PartHeaderContentPresenter = "HeaderContentPresenter";
        private const string NarrowState = "NarrowState";
        private const string WideState = "WideState";
        private const string WidthStates = "WidthStates";
        private const string SelectionStates = "SelectionStates";
        private const string HasSelectionState = "HasSelection";
        private const string NoSelectionNarrowState = "NoSelectionNarrow";
        private const string NoSelectionWideState = "NoSelectionWide";
        private const double WideStateMinWidth = 720;

        private AppViewBackButtonVisibility _previousBackButtonVisibility;
        private ContentPresenter _detailsPresenter;
        //private VisualStateGroup _stateGroup;
        //private VisualState _narrowState;
        private Frame _frame;
        //private bool _loaded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDetailsView"/> class.
        /// </summary>
        public MasterDetailsView()
        {
            DefaultStyleKey = typeof(MasterDetailsView);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _detailsPresenter = (ContentPresenter)GetTemplateChild(PartDetailsPresenter);
            SetDetailsContent();

            SetMasterHeaderVisibility();
            OnDetailsCommandBarChanged();
            OnMasterCommandBarChanged();

            SizeChanged -= MasterDetailsView_SizeChanged;
            SizeChanged += MasterDetailsView_SizeChanged;

            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            UpdateView(true);

            //var nop = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            //{
            //    _frame = null;
            //    _frame = await GetFrame();
            //    _frame.Navigating += OnFrameNavigating;
            //});

            //if (_loaded && GetStateGroup() == null)
            //{
            //    _stateGroup = (VisualStateGroup)GetTemplateChild(WidthStates);
            //    if (_stateGroup != null)
            //    {
            //        _stateGroup.CurrentStateChanged += OnVisualStateChanged;
            //        _narrowState = GetTemplateChild(NarrowState) as VisualState;
            //    }
            //}
        }

        private void MasterDetailsView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateView(true);
        }

        //private VisualStateGroup GetStateGroup()
        //{
        //    if (_stateGroup == null)
        //    {
        //        _stateGroup = (VisualStateGroup)GetTemplateChild(WidthStates);
        //    }

        //    return _stateGroup;
        //}

        /// <summary>
        /// Fired when the SelectedItem changes.
        /// </summary>
        /// <param name="d">The sender</param>
        /// <param name="e">The event args</param>
        /// <remarks>
        /// Sets up animations for the DetailsPresenter for animating in/out.
        /// </remarks>
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (MasterDetailsView)d;

            view.OnSelectionChanged(new SelectionChangedEventArgs(new List<object> { e.OldValue }, new List<object> { e.NewValue }));

            view.UpdateView(true);

            // If there is no selection, do not remove the DetailsPresenter content but let it animate out.
            if (view.SelectedItem != null)
            {
                view.SetDetailsContent();
            }
        }

        /// <summary>
        /// Fired when the <see cref="MasterHeader"/> is changed.
        /// </summary>
        /// <param name="d">The sender</param>
        /// <param name="e">The event args</param>
        private static void OnMasterHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (MasterDetailsView)d;
            view.SetMasterHeaderVisibility();
        }

        /// <summary>
        /// Fired when the DetailsCommandBar changes.
        /// </summary>
        /// <param name="d">The sender</param>
        /// <param name="e">The event args</param>
        private static void OnDetailsCommandBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (MasterDetailsView)d;
            view.OnDetailsCommandBarChanged();
        }

        /// <summary>
        /// Fired when the MasterCommandBar changes.
        /// </summary>
        /// <param name="d">The sender</param>
        /// <param name="e">The event args</param>
        private static void OnMasterCommandBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (MasterDetailsView)d;
            view.OnMasterCommandBarChanged();
        }

        // Have to wait to get the VisualStateGroup until the control has Loaded
        // If we try to get the VisualStateGroup in the OnApplyTemplate the
        // CurrentStateChanged event does not fire properly
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignMode.DesignModeEnabled == false)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                if (_frame != null)
                {
                    _frame.Navigating -= OnFrameNavigating;
                }

                _frame = this.FindAscendant<Frame>();
                _frame.Navigating += OnFrameNavigating;
            }

            //    //if (_stateGroup != null)
            //    //{
            //    //    _stateGroup.CurrentStateChanged -= OnVisualStateChanged;
            //    //}

            //    //if (GetStateGroup() != null)
            //    //{
            //    //    _stateGroup.CurrentStateChanged += OnVisualStateChanged;
            //    //    _narrowState = GetTemplateChild(NarrowState) as VisualState;
            //    //    UpdateView(true);
            //    //}

            //    //_loaded = true;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (DesignMode.DesignModeEnabled == false)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
                if (_frame != null)
                {
                    _frame.Navigating -= OnFrameNavigating;
                }
            }

            //    if (_stateGroup != null)
            //    {
            //        _stateGroup.CurrentStateChanged -= OnVisualStateChanged;
            //        _stateGroup = null;
            //    }
        }

        /// <summary>
        /// Fires when the addaptive trigger changes state.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        /// <remarks>
        /// Handles showing/hiding the back button when the state changes
        /// </remarks>
        private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateView(false);
        }

        /// <summary>
        /// Closes the details pane if we are in narrow state
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The event args</param>
        private void OnFrameNavigating(object sender, NavigatingCancelEventArgs args)
        {
            if ((args.NavigationMode == NavigationMode.Back) && (ViewState == MasterDetailsViewState.Details))
            {
                SelectedItem = null;
                args.Cancel = true;
            }
        }

        /// <summary>
        /// Closes the details pane if we are in narrow state
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The event args</param>
        private void OnBackRequested(object sender, BackRequestedEventArgs args)
        {
            if (ViewState == MasterDetailsViewState.Details)
            {
                // let the OnFrameNavigating method handle it if
                if (_frame == null || !_frame.CanGoBack)
                {
                    SelectedItem = null;
                }

                args.Handled = true;
            }
        }

        private void SetMasterHeaderVisibility()
        {
            var headerPresenter = GetTemplateChild(PartHeaderContentPresenter) as FrameworkElement;
            if (headerPresenter != null)
            {
                headerPresenter.Visibility = MasterHeader != null
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void UpdateView(bool animate)
        {
            UpdateViewState();
            SetBackButtonVisibility(ViewState);
            SetVisualState(animate);
        }

        /// <summary>
        /// Sets the back button visibility based on the current visual state and selected item
        /// </summary>
        private void SetBackButtonVisibility(MasterDetailsViewState previousState)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (ViewState == MasterDetailsViewState.Details)
            {
                var navigationManager = SystemNavigationManager.GetForCurrentView();
                _previousBackButtonVisibility = navigationManager.AppViewBackButtonVisibility;

                navigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else if (previousState == MasterDetailsViewState.Details)
            {
                // Make sure we show the back button if the stack can navigate back
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = _previousBackButtonVisibility;
            }
        }

        //private async Task<Frame> GetFrame()
        //{
        //    if (_frame == null)
        //    {
        //        _frame = this.FindAscendant<Frame>();

        //        if (_frame == null)
        //        {
        //            var taskSource = new TaskCompletionSource<object>();
        //            RoutedEventHandler handler = null;
        //            handler = (s, args) =>
        //            {
        //                Loaded -= handler;
        //                _frame = this.FindAscendant<Frame>();
        //                taskSource.SetResult(null);
        //            };

        //            Loaded += handler;

        //            await taskSource.Task;
        //        }
        //    }

        //    return _frame;
        //}

        private void UpdateViewState()
        {
            //if (GetStateGroup() == null)
            //{
            //    return;
            //}

            var before = ViewState;

            if (ActualWidth < WideStateMinWidth)
            {
                ViewState = SelectedItem == null ? MasterDetailsViewState.Master : MasterDetailsViewState.Details;
            }
            else
            {
                ViewState = MasterDetailsViewState.Both;
            }

            var after = ViewState;

            if (before != after)
            {
                ViewStateChanged?.Invoke(this, after);
            }
        }

        private void SetVisualState(bool animate)
        {
            string noSelectionState = ActualWidth < WideStateMinWidth
                ? NoSelectionNarrowState
                : NoSelectionWideState;
            VisualStateManager.GoToState(this, SelectedItem == null ? noSelectionState : HasSelectionState, animate);
            VisualStateManager.GoToState(this, ActualWidth < WideStateMinWidth ? NarrowState : WideState, animate);
        }

        private void SetDetailsContent()
        {
            if (_detailsPresenter != null)
            {
                _detailsPresenter.Content = MapDetails == null
                    ? SelectedItem
                    : SelectedItem != null ? MapDetails(SelectedItem) : null;
            }
        }

        private void OnMasterCommandBarChanged()
        {
            var panel = GetTemplateChild("DetailsCommandBarPanel") as Panel;
            if (panel == null)
            {
                return;
            }

            panel.Children.Clear();
            if (DetailsCommandBar != null)
            {
                panel.Children.Add(DetailsCommandBar);
            }
        }

        private void OnDetailsCommandBarChanged()
        {
            var panel = GetTemplateChild("MasterCommandBarPanel") as Panel;
            if (panel == null)
            {
                return;
            }

            panel.Children.Clear();
            if (MasterCommandBar != null)
            {
                panel.Children.Add(MasterCommandBar);
            }
        }
    }
}