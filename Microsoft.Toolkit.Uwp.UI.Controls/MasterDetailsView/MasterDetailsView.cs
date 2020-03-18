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
    [TemplatePart(Name = PartDetailsPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartDetailsPane, Type = typeof(FrameworkElement))]
    [TemplateVisualState(Name = NoSelectionNarrowState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = NoSelectionWideState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = HasSelectionWideState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = HasSelectionNarrowState, GroupName = SelectionStates)]
    public partial class MasterDetailsView : ItemsControl
    {
        // All view states:
        private const string SelectionStates = "SelectionStates";
        private const string NoSelectionWideState = "NoSelectionWide";
        private const string HasSelectionWideState = "HasSelectionWide";
        private const string NoSelectionNarrowState = "NoSelectionNarrow";
        private const string HasSelectionNarrowState = "HasSelectionNarrow";

        private const string HasItemsStates = "HasItemsStates";
        private const string HasItemsState = "HasItemsState";
        private const string HasNoItemsState = "HasNoItemsState";

        // Control names:
        private const string PartRootPane = "RootPane";
        private const string PartDetailsPresenter = "DetailsPresenter";
        private const string PartDetailsPane = "DetailsPane";
        private const string PartMasterList = "MasterList";
        private const string PartBackButton = "MasterDetailsBackButton";
        private const string PartHeaderContentPresenter = "HeaderContentPresenter";
        private const string PartMasterCommandBarPanel = "MasterCommandBarPanel";
        private const string PartDetailsCommandBarPanel = "DetailsCommandBarPanel";

        /// <summary>
        /// Used to prevent screen flickering if only the order of the selected item changed.
        /// </summary>
        private bool _ignoreClearSelectedItem;

        private ContentPresenter _detailsPresenter;
        private Microsoft.UI.Xaml.Controls.TwoPaneView _twoPaneView;
        private VisualStateGroup _selectionStateGroup;

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
        /// Updates the visual state of the control.
        /// </summary>
        /// <param name="animate">False to skip animations.</param>
        private void SetVisualState(bool animate)
        {
            string noSelectionState;
            string hasSelectionState;
            if (ViewState == MasterDetailsViewState.Both)
            {
                noSelectionState = NoSelectionWideState;
                hasSelectionState = HasSelectionWideState;
            }
            else
            {
                noSelectionState = NoSelectionNarrowState;
                hasSelectionState = HasSelectionNarrowState;
            }

            VisualStateManager.GoToState(this, SelectedItem is null ? noSelectionState : hasSelectionState, animate);
            VisualStateManager.GoToState(this, Items.Count > 0 ? HasItemsState : HasNoItemsState, animate);
        }

        /// <summary>
        /// Sets the content of the <see cref="SelectedItem"/> based on current <see cref="MapDetails"/> function.
        /// </summary>
        private void SetDetailsContent()
        {
            if (_detailsPresenter != null)
            {
                // Update the content template:
                if (!(_detailsPresenter.ContentTemplateSelector is null))
                {
                    _detailsPresenter.ContentTemplate = _detailsPresenter.ContentTemplateSelector.SelectTemplate(SelectedItem, _detailsPresenter);
                }
                // Update the content:
                _detailsPresenter.Content = MapDetails is null
                    ? SelectedItem
                    : !(SelectedItem is null) ? MapDetails(SelectedItem) : null;

            }
        }

        private void SetMasterHeaderVisibility()
        {
            if (GetTemplateChild(PartHeaderContentPresenter) is FrameworkElement headerPresenter)
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
            _ignoreClearSelectedItem = true;
            SelectedItem = null;
            _ignoreClearSelectedItem = false;
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
            OnCommandBarChanged(PartMasterCommandBarPanel, MasterCommandBar);
        }

        private void OnDetailsCommandBarChanged()
        {
            OnCommandBarChanged(PartDetailsCommandBarPanel, DetailsCommandBar);
        }

        private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
        {
            // Prevent setting the SelectedItem to null if only the order changed (=> collection reset got triggered).
            if (!_ignoreClearSelectedItem && !(e.OldValue is null) && e.NewValue is null && Items.Contains(e.OldValue))
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

            if (_twoPaneView is null)
            {
                ViewState = MasterDetailsViewState.Both;
            }

            // Single pane:
            else if (_twoPaneView.Mode == Microsoft.UI.Xaml.Controls.TwoPaneViewMode.SinglePane)
            {
                ViewState = SelectedItem is null ? MasterDetailsViewState.Master : MasterDetailsViewState.Details;
                _twoPaneView.PanePriority = SelectedItem is null ? Microsoft.UI.Xaml.Controls.TwoPaneViewPriority.Pane1 : Microsoft.UI.Xaml.Controls.TwoPaneViewPriority.Pane2;
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
            if (GetTemplateChild(PartDetailsPane) is DependencyObject details)
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
            if (GetTemplateChild(PartMasterList) is Control masterList)
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
            if (GetTemplateChild(PartMasterCommandBarPanel) is ListViewBase masterList)
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

            if (!(_inlineBackButton is null))
            {
                _inlineBackButton.Click -= OnInlineBackButtonClicked;
            }

            _inlineBackButton = (Button)GetTemplateChild(PartBackButton);
            if (!(_inlineBackButton is null))
            {
                _inlineBackButton.Click += OnInlineBackButtonClicked;
            }

            _twoPaneView = (Microsoft.UI.Xaml.Controls.TwoPaneView)GetTemplateChild(PartRootPane);
            if (!(_twoPaneView is null))
            {
                _twoPaneView.ModeChanged += OnModeChanged;
            }

            _detailsPresenter = (ContentPresenter)GetTemplateChild(PartDetailsPresenter);

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
                if (!(_frame is null))
                {
                    _frame.Navigating -= OnFrameNavigating;
                }

                _selectionStateGroup = (VisualStateGroup)GetTemplateChild(SelectionStates);
                if (!(_selectionStateGroup is null))
                {
                    _selectionStateGroup.CurrentStateChanged -= OnSelectionStateChanged;
                    _selectionStateGroup = null;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!DesignMode.DesignModeEnabled)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                if (!(_frame is null))
                {
                    _frame.Navigating -= OnFrameNavigating;
                }

                _navigationView = this.FindAscendants().FirstOrDefault(p => p.GetType().FullName == "Microsoft.UI.Xaml.Controls.NavigationView");
                _frame = this.FindAscendant<Frame>();
                if (!(_frame is null))
                {
                    _frame.Navigating += OnFrameNavigating;
                }

                _selectionStateGroup = (VisualStateGroup)GetTemplateChild(SelectionStates);
                if (!(_selectionStateGroup is null))
                {
                    _selectionStateGroup.CurrentStateChanged += OnSelectionStateChanged;
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
