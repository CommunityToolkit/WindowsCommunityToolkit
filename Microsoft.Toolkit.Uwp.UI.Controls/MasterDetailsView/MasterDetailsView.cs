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
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

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
        private const string NoSelectionNarrowState = "NoSelectionNarrow";
        private const string NoSelectionWideState = "NoSelectionWide";

        private ContentPresenter _detailsPresenter;
        private VisualStateGroup _stateGroup;
        private VisualState _narrowState;
        private Frame _frame;
        private Visual _root;
        private Compositor _compositor;
        private Visual _detailsVisual;

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

            var detailsPanel = (FrameworkElement)GetTemplateChild(PartDetailsPanel);
            _root = ElementCompositionPreview.GetElementVisual(detailsPanel);
            _compositor = _root.Compositor;

            _detailsPresenter = (ContentPresenter)GetTemplateChild(PartDetailsPresenter);
            _detailsPresenter.SizeChanged += OnSizeChanged;
            _detailsVisual = ElementCompositionPreview.GetElementVisual(_detailsPresenter);
            SetDetailsOffset();

            SetMasterHeaderVisibility();
        }

        /// <summary>
        /// Fired when the SelectedItem changes.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Sets up animations for the DetailsPresenter for animating in/out.
        /// </remarks>
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (MasterDetailsView)d;
            string noSelectionState = view._stateGroup.CurrentState == view._narrowState
                ? NoSelectionNarrowState
                : NoSelectionWideState;
            VisualStateManager.GoToState(view, view.SelectedItem == null ? noSelectionState : "HasSelection", true);

            view.OnSelectionChanged(new SelectionChangedEventArgs(new List<object> { e.OldValue }, new List<object> { e.NewValue }));

            if (view.SelectedItem != null)
            {
                // Move the visual to the side so it can animate back in
                view._detailsVisual.Offset = new Vector3((float)view._detailsPresenter.ActualWidth, 0, 0);
            }

            view._detailsPresenter.Content = view.MapDetails == null
                ? view.SelectedItem
                : view.MapDetails(view.SelectedItem);

            // determine the animate to create. If the SelectedItem is null we
            // want to animate the content out. If the SelectedItem is not null
            // we want to animate the content in
            Vector3 offset = view.SelectedItem == null
                ? new Vector3((float)view._detailsPresenter.ActualWidth, 0, 0)
                : new Vector3(-(float)view._detailsPresenter.ActualWidth, 0, 0);
            view.AnimateFromCurrentByValue(view._detailsVisual, offset);
            view.SetBackButtonVisibility(view._stateGroup.CurrentState);
        }

        /// <summary>
        /// Fired when the <see cref="MasterHeader"/> is changed.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMasterHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (MasterDetailsView)d;
            view.SetMasterHeaderVisibility();
        }

        // Have to wait to get the VisualStateGroup until the control has Loaded
        // If we try to get the VisualStateGroup in the OnApplyTemplate the
        // CurrentStateChanged event does not fire properly
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            if (_stateGroup != null)
            {
                _stateGroup.CurrentStateChanged -= OnVisualStateChanged;
            }

            _stateGroup = (VisualStateGroup)GetTemplateChild(WidthStates);
            _stateGroup.CurrentStateChanged += OnVisualStateChanged;

            _narrowState = GetTemplateChild(NarrowState) as VisualState;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
        }

        /// <summary>
        /// Fires when the addaptive trigger changes state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Handles showing/hiding the back button when the state changes
        /// </remarks>
        private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            SetBackButtonVisibility(e.NewState);
        }

        /// <summary>
        /// Fires when the size of the control changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Handles setting the Offset of the DetailsPresenter if there is no SelectedItem
        /// </remarks>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetDetailsOffset();
        }

        /// <summary>
        /// Closes the details pane if we are in narrow state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnBackRequested(object sender, BackRequestedEventArgs args)
        {
            if (((_stateGroup.CurrentState == _narrowState) || (_stateGroup.CurrentState == null)) && (SelectedItem != null))
            {
                SelectedItem = null;
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

        private void SetDetailsOffset()
        {
            if (SelectedItem == null)
            {
                _detailsVisual.Offset = new Vector3((float)_detailsPresenter.ActualWidth, 0, 0);
            }
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

        // Creates and defines the Keyframe animation using a current value of target Visual and animating by a value
        private void AnimateFromCurrentByValue(Visual targetVisual, Vector3 delta)
        {
            var animation = _compositor.CreateVector3KeyFrameAnimation();

            // Utilize a current value of the target visual in Expression KeyFrame and modify by a value 
            animation.InsertExpressionKeyFrame(1.00f, "this.StartingValue + delta");

            // Define the value variable
            animation.SetVector3Parameter("delta", delta);
            animation.Duration = TimeSpan.FromMilliseconds(250);

            targetVisual.StartAnimation("Offset", animation);
        }
    }
}
