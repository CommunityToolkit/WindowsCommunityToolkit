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
        private const float AnimationOffset = 200;

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
            _detailsVisual = ElementCompositionPreview.GetElementVisual(_detailsPresenter);
            if (SelectedItem == null)
            {
                _detailsVisual.Opacity = 0;
            }

            SetMasterHeaderVisibility();
        }

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
            string noSelectionState = view._stateGroup.CurrentState == view._narrowState
                ? NoSelectionNarrowState
                : NoSelectionWideState;
            VisualStateManager.GoToState(view, view.SelectedItem == null ? noSelectionState : "HasSelection", true);

            view.OnSelectionChanged(new SelectionChangedEventArgs(new List<object> { e.OldValue }, new List<object> { e.NewValue }));

            if (view.SelectedItem != null)
            {
                // Move the visual to the side so it can animate back in
                view._detailsVisual.Offset = new Vector3(AnimationOffset, 0, 0);
                view._detailsVisual.Opacity = 0;
            }

            view._detailsPresenter.Content = view.MapDetails == null
                ? view.SelectedItem
                : view.MapDetails(view.SelectedItem);

            // determine the animate to create. If the SelectedItem is null we
            // want to animate the content out. If the SelectedItem is not null
            // we want to animate the content in
            Vector3 offset = view.SelectedItem == null
                ? new Vector3(AnimationOffset, 0, 0)
                : new Vector3(0, 0, 0);
            float opacity = view.SelectedItem == null
                ? 0.0f : 1.0f;
            view.AnimateFromCurrentToValue(view._detailsVisual, offset, opacity);
            view.SetBackButtonVisibility(view._stateGroup.CurrentState);
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

            UpdateViewState();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
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
            SetBackButtonVisibility(e.NewState);
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

        /// <summary>
        /// Sets the back button visibility based on the current visual state and selected item
        /// </summary>
        private void SetBackButtonVisibility(VisualState currentState)
        {
            UpdateViewState();

            if (ViewState == MasterDetailsViewState.Details)
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

        // Creates and defines the Keyframe animation animating to a value
        private void AnimateFromCurrentToValue(Visual targetVisual, Vector3 offset, float opacity)
        {
            var animationOffset = _compositor.CreateVector3KeyFrameAnimation();

            animationOffset.InsertKeyFrame(1.00f, offset);
            animationOffset.Duration = TimeSpan.FromMilliseconds(250);
            animationOffset.Target = "Offset";

            var animationOpacity = _compositor.CreateScalarKeyFrameAnimation();

            animationOpacity.InsertKeyFrame(1.00f, opacity);
            animationOpacity.Duration = animationOffset.Duration;
            animationOpacity.Target = "Opacity";

            var animations = _compositor.CreateAnimationGroup();
            animations.Add(animationOffset);
            animations.Add(animationOpacity);
            targetVisual.StartAnimationGroup(animations);
        }

        private void UpdateViewState()
        {
            var before = ViewState;

            if (_stateGroup.CurrentState == _narrowState || _stateGroup.CurrentState == null)
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
    }
}
