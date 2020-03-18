// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Extensions;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Panel that allows for a Master/Details pattern.
    /// </summary>
    [TemplatePart(Name = PART_DETAILS_PRESENTER, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_DETAILS_PANE, Type = typeof(FrameworkElement))]
    [TemplateVisualState(Name = NO_SELECTION_NARROW, GroupName = SELECTION_STATES)]
    [TemplateVisualState(Name = NO_SELECTION_WIDE, GroupName = SELECTION_STATES)]
    [TemplateVisualState(Name = HAS_SELECTION_WIDE, GroupName = SELECTION_STATES)]
    [TemplateVisualState(Name = HAS_SELECTION_NARROW, GroupName = SELECTION_STATES)]
    public partial class MasterDetailsView : ItemsControl
    {
        // All view states:
        private const string SELECTION_STATES = "SelectionStates";
        private const string NO_SELECTION_WIDE = "NoSelectionWide";
        private const string HAS_SELECTION_WIDE = "HasSelectionWide";
        private const string NO_SELECTION_NARROW = "NoSelectionNarrow";
        private const string HAS_SELECTION_NARROW = "HasSelectionNarrow";

        private const string HAS_ITEMS_STATES = "HasItemsStates";
        private const string HAS_ITEMS_STATE = "HasItemsState";
        private const string HAS_NO_ITEMS_STATE = "HasNoItemsState";

        // Control names:
        private const string PART_ROOT_PANE = "RootPane";
        private const string PART_DETAILS_PRESENTER = "DetailsPresenter";
        private const string PART_DETAILS_PANE = "DetailsPane";
        private const string PART_MASTER_LIST = "MasterList";
        private const string PART_BACK_BUTTON = "MasterDetailsBackButton";
        private const string PART_HEADER_CONTENT_PRESENTER = "HeaderContentPresenter";
        private const string PART_MASTER_COMMAND_BAR = "MasterCommandBarPanel";
        private const string PART_DETAILS_COMMAND_BAR = "DetailsCommandBarPanel";

        /// <summary>
        /// Used to prevent screen flickering if only the order of the selected item changed.
        /// </summary>
        private bool ignoreClearSelectedItem;

        private ContentPresenter detailsPresenter;
        private Microsoft.UI.Xaml.Controls.TwoPaneView twoPaneView;
        private VisualStateGroup selectionStateGroup;

        public MasterDetailsView()
        {
            DefaultStyleKey = typeof(MasterDetailsView);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// Updates the visual state of the control.
        /// </summary>
        /// <param name="animate">False to skip animations.</param>
        private void SetVisualState(bool animate)
        {
            string noSelectionState;
            string hasSelectionState;
            if (ViewState == MasterDetailsViewState.Both)
            {
                noSelectionState = NO_SELECTION_WIDE;
                hasSelectionState = HAS_SELECTION_WIDE;
            }
            else
            {
                noSelectionState = NO_SELECTION_NARROW;
                hasSelectionState = HAS_SELECTION_NARROW;
            }

            VisualStateManager.GoToState(this, SelectedItem is null ? noSelectionState : hasSelectionState, animate);
            VisualStateManager.GoToState(this, Items.Count > 0 ? HAS_ITEMS_STATE : HAS_NO_ITEMS_STATE, animate);
        }

        /// <summary>
        /// Sets the content of the <see cref="SelectedItem"/> based on current <see cref="MapDetails"/> function.
        /// </summary>
        private void SetDetailsContent()
        {
            if (detailsPresenter != null)
            {
                // Update the content template:
                if (!(detailsPresenter.ContentTemplateSelector is null))
                {
                    detailsPresenter.ContentTemplate = detailsPresenter.ContentTemplateSelector.SelectTemplate(SelectedItem, detailsPresenter);
                }
                // Update the content:
                detailsPresenter.Content = MapDetails is null
                    ? SelectedItem
                    : !(SelectedItem is null) ? MapDetails(SelectedItem) : null;

            }
        }

        private void SetMasterHeaderVisibility()
        {
            if (GetTemplateChild(PART_HEADER_CONTENT_PRESENTER) is FrameworkElement headerPresenter)
            {
                headerPresenter.Visibility = MasterHeader != null
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Clears the <see cref="SelectedItem"/> and prevent flickering of the UI if only the order of the items changed.
        /// </summary>
        public void ClearSelectedItem()
        {
            ignoreClearSelectedItem = true;
            SelectedItem = null;
            ignoreClearSelectedItem = false;
        }

        private void OnCommandBarChanged(string panelName, CommandBar commandbar)
        {
            if (!(GetTemplateChild(panelName) is Panel panel))
            {
                return;
            }

            panel.Children.Clear();
            if (commandbar != null)
            {
                panel.Children.Add(commandbar);
            }
        }

        private void OnMasterCommandBarChanged()
        {
            OnCommandBarChanged(PART_MASTER_COMMAND_BAR, MasterCommandBar);
        }

        private void OnDetailsCommandBarChanged()
        {
            OnCommandBarChanged(PART_DETAILS_COMMAND_BAR, DetailsCommandBar);
        }

        private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
        {
            // Prevent setting the SelectedItem to null if only the order changed (=> collection reset got triggered).
            if (!ignoreClearSelectedItem && !(e.OldValue is null) && e.NewValue is null && Items.Contains(e.OldValue))
            {
                SelectedItem = e.OldValue;
                return;
            }

            OnSelectionChanged(new SelectionChangedEventArgs(new List<object> { e.OldValue }, new List<object> { e.NewValue }));

            UpdateView(true);

            // If there is no selection, do not remove the DetailsPresenter content but let it animate out.
            if (!(SelectedItem is null))
            {
                SetDetailsContent();
            }
        }

        private void UpdateView(bool animate)
        {
            UpdateViewState();
            SetVisualState(animate);
        }

        private void UpdateViewState()
        {
            MasterDetailsViewState previousState = ViewState;

            if (twoPaneView is null)
            {
                ViewState = MasterDetailsViewState.Both;
            }

            // Single pane:
            else if (twoPaneView.Mode == Microsoft.UI.Xaml.Controls.TwoPaneViewMode.SinglePane)
            {
                ViewState = SelectedItem is null ? MasterDetailsViewState.Master : MasterDetailsViewState.Details;
                twoPaneView.PanePriority = SelectedItem is null ? Microsoft.UI.Xaml.Controls.TwoPaneViewPriority.Pane1 : Microsoft.UI.Xaml.Controls.TwoPaneViewPriority.Pane2;
            }

            // Dual pane:
            else
            {
                ViewState = MasterDetailsViewState.Both;
            }

            if (previousState != ViewState)
            {
                ViewStateChanged?.Invoke(this, ViewState);
                SetBackButtonVisibility(previousState);
            }
        }

        /// <summary>
        /// Sets focus to the relevant control based on the viewState.
        /// </summary>
        /// <param name="viewState">the view state</param>
        private void SetFocus(MasterDetailsViewState viewState)
        {
            if (viewState != MasterDetailsViewState.Details)
            {
                FocusItemList();
            }
            else
            {
                FocusFirstFocusableElementInDetails();
            }
        }

        /// <summary>
        /// Sets focus to the first focusable element in the details template
        /// </summary>
        private void FocusFirstFocusableElementInDetails()
        {
            if (GetTemplateChild(PART_DETAILS_PANE) is DependencyObject details)
            {
                DependencyObject focusableElement = FocusManager.FindFirstFocusableElement(details);
                (focusableElement as Control)?.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Sets focus to the item list
        /// </summary>
        private void FocusItemList()
        {
            if (GetTemplateChild(PART_MASTER_LIST) is Control masterList)
            {
                masterList.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Sets whether the selected item should change when focused with the keyboard based on the view state
        /// </summary>
        /// <param name="viewState">the view state</param>
        private void SetListSelectionWithKeyboardFocusOnVisualStateChanged(MasterDetailsViewState viewState)
        {
            if (viewState == MasterDetailsViewState.Both)
            {
                SetListSelectionWithKeyboardFocus(true);
            }
            else
            {
                SetListSelectionWithKeyboardFocus(false);
            }
        }

        /// <summary>
        /// Sets whether the selected item should change when focused with the keyboard
        /// </summary>
        private void SetListSelectionWithKeyboardFocus(bool singleSelectionFollowsFocus)
        {
            if (GetTemplateChild(PART_MASTER_COMMAND_BAR) is ListViewBase masterList)
            {
                masterList.SingleSelectionFollowsFocus = singleSelectionFollowsFocus;
            }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!(inlineBackButton is null))
            {
                inlineBackButton.Click -= OnInlineBackButtonClicked;
            }

            inlineBackButton = (Button)GetTemplateChild(PART_BACK_BUTTON);
            if (!(inlineBackButton is null))
            {
                inlineBackButton.Click += OnInlineBackButtonClicked;
            }

            twoPaneView = (Microsoft.UI.Xaml.Controls.TwoPaneView)GetTemplateChild(PART_ROOT_PANE);
            if (!(twoPaneView is null))
            {
                twoPaneView.ModeChanged += OnModeChanged;
            }

            detailsPresenter = (ContentPresenter)GetTemplateChild(PART_DETAILS_PRESENTER);

            SetDetailsContent();

            SetMasterHeaderVisibility();
            OnDetailsCommandBarChanged();
            OnMasterCommandBarChanged();

            UpdateView(true);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (!DesignMode.DesignModeEnabled)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
                if (!(frame is null))
                {
                    frame.Navigating -= OnFrameNavigating;
                }

                selectionStateGroup = (VisualStateGroup)GetTemplateChild(SELECTION_STATES);
                if (!(selectionStateGroup is null))
                {
                    selectionStateGroup.CurrentStateChanged -= OnSelectionStateChanged;
                    selectionStateGroup = null;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!DesignMode.DesignModeEnabled)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                if (!(frame is null))
                {
                    frame.Navigating -= OnFrameNavigating;
                }

                navigationView = this.FindAscendants().FirstOrDefault(p => p.GetType().FullName == "Microsoft.UI.Xaml.Controls.NavigationView");
                frame = this.FindAscendant<Frame>();
                if (!(frame is null))
                {
                    frame.Navigating += OnFrameNavigating;
                }

                selectionStateGroup = (VisualStateGroup)GetTemplateChild(SELECTION_STATES);
                if (!(selectionStateGroup is null))
                {
                    selectionStateGroup.CurrentStateChanged += OnSelectionStateChanged;
                }
            }
        }

        private void OnModeChanged(Microsoft.UI.Xaml.Controls.TwoPaneView sender, object args)
        {
            UpdateView(true);
        }

        /// <summary>
        /// Fires when the selection state of the control changes
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event args</param>
        /// <remarks>
        /// Sets focus to the item list when the viewState is not Details.
        /// Sets whether the selected item should change when focused with the keyboard.
        /// </remarks>
        private void OnSelectionStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            SetFocus(ViewState);
            SetListSelectionWithKeyboardFocusOnVisualStateChanged(ViewState);
        }
    }
}
