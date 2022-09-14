// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using NavigationView = Microsoft.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Panel that allows for a List/Details pattern.
    /// </summary>
    [TemplatePart(Name = PartDetailsPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartDetailsPanel, Type = typeof(FrameworkElement))]
    [TemplateVisualState(Name = NoSelectionNarrowState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = NoSelectionWideState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = HasSelectionWideState, GroupName = SelectionStates)]
    [TemplateVisualState(Name = HasSelectionNarrowState, GroupName = SelectionStates)]
    public partial class ListDetailsView : ItemsControl
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
        private const string PartRootPanel = "RootPanel";
        private const string PartDetailsPresenter = "DetailsPresenter";
        private const string PartDetailsPanel = "DetailsPanel";
        private const string PartMainList = "MainList";
        private const string PartBackButton = "ListDetailsBackButton";
        private const string PartHeaderContentPresenter = "HeaderContentPresenter";
        private const string PartListPaneCommandBarPanel = "ListPaneCommandBarPanel";
        private const string PartDetailsPaneCommandBarPanel = "DetailsPaneCommandBarPanel";

        private ContentPresenter _detailsPresenter;
        private NavigationView.TwoPaneView _twoPaneView;
        private VisualStateGroup _selectionStateGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDetailsView"/> class.
        /// </summary>
        public ListDetailsView()
        {
            DefaultStyleKey = typeof(ListDetailsView);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// Clears the <see cref="SelectedItem"/> and prevent flickering of the UI if only the order of the items changed.
        /// </summary>
        public void ClearSelectedItem()
        {
            SelectedItem = null;
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_inlineBackButton != null)
            {
                _inlineBackButton.Click -= OnInlineBackButtonClicked;
            }

            _inlineBackButton = (Button)GetTemplateChild(PartBackButton);
            if (_inlineBackButton != null)
            {
                _inlineBackButton.Click += OnInlineBackButtonClicked;
            }

            _selectionStateGroup = (VisualStateGroup)GetTemplateChild(SelectionStates);
            if (_selectionStateGroup != null)
            {
                _selectionStateGroup.CurrentStateChanged += OnSelectionStateChanged;
            }

            _twoPaneView = (NavigationView.TwoPaneView)GetTemplateChild(PartRootPanel);
            if (_twoPaneView != null)
            {
                _twoPaneView.ModeChanged += OnModeChanged;
            }

            _detailsPresenter = (ContentPresenter)GetTemplateChild(PartDetailsPresenter);
            SetDetailsContent();

            SetListHeaderVisibility();
            OnDetailsCommandBarChanged();
            OnListCommandBarChanged();
            OnListPaneWidthChanged();

            UpdateView(true);
        }

        /// <summary>
        /// Fired when the SelectedIndex changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <remarks>
        /// Sets up animations for the DetailsPresenter for animating in/out.
        /// </remarks>
        private void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is int newIndex)
            {
                object newItem = newIndex >= 0 && Items.Count > newIndex ? Items[newIndex] : null;
                object oldItem = e.OldValue is int oldIndex && oldIndex >= 0 && Items.Count > oldIndex ? Items[oldIndex] : null;
                if (SelectedItem != newItem)
                {
                    if (newItem is null)
                    {
                        ClearSelectedItem();
                    }
                    else
                    {
                        SetValue(SelectedItemProperty, newItem);
                        UpdateSelection(oldItem, newItem);
                    }
                }
            }
        }

        /// <summary>
        /// Fired when the SelectedItem changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <remarks>
        /// Sets up animations for the DetailsPresenter for animating in/out.
        /// </remarks>
        private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
        {
            int index = SelectedItem is null ? -1 : Items.IndexOf(SelectedItem);

            // If there is no selection, do not remove the DetailsPresenter content but let it animate out.
            if (index >= 0)
            {
                SetDetailsContent();
            }

            if (SelectedIndex != index)
            {
                SetValue(SelectedIndexProperty, index);
                UpdateSelection(e.OldValue, e.NewValue);
            }

            OnSelectionChanged(new SelectionChangedEventArgs(new List<object> { e.OldValue }, new List<object> { e.NewValue }));
            UpdateView(true);
            SetFocus(ViewState);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!DesignMode.DesignModeEnabled)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                if (_frame != null)
                {
                    _frame.Navigating -= OnFrameNavigating;
                }

                _navigationView = this.FindAscendant<NavigationView.NavigationView>();
                _frame = this.FindAscendant<Frame>();
                if (_frame != null)
                {
                    _frame.Navigating += OnFrameNavigating;
                }
            }
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

                _selectionStateGroup = (VisualStateGroup)GetTemplateChild(SelectionStates);
                if (_selectionStateGroup != null)
                {
                    _selectionStateGroup.CurrentStateChanged -= OnSelectionStateChanged;
                    _selectionStateGroup = null;
                }
            }
        }

        /// <summary>
        /// Raises SelectionChanged event and updates view.
        /// </summary>
        /// <param name="oldSelection">Old selection.</param>
        /// <param name="newSelection">New selection.</param>
        private void UpdateSelection(object oldSelection, object newSelection)
        {
            OnSelectionChanged(new SelectionChangedEventArgs(new List<object> { oldSelection }, new List<object> { newSelection }));

            UpdateView(true);

            // If there is no selection, do not remove the DetailsPresenter content but let it animate out.
            if (SelectedItem != null)
            {
                SetDetailsContent();
            }
        }

        private void SetListHeaderVisibility()
        {
            if (GetTemplateChild(PartHeaderContentPresenter) is FrameworkElement headerPresenter)
            {
                headerPresenter.Visibility = ListHeader != null
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void UpdateView(bool animate)
        {
            UpdateViewState();
            SetVisualState(animate);
        }

        private void UpdateViewState()
        {
            ListDetailsViewState previousState = ViewState;

            if (_twoPaneView == null)
            {
                ViewState = ListDetailsViewState.Both;
            }

            // Single pane:
            else if (_twoPaneView.Mode == NavigationView.TwoPaneViewMode.SinglePane)
            {
                ViewState = SelectedItem == null ? ListDetailsViewState.List : ListDetailsViewState.Details;
                _twoPaneView.PanePriority = SelectedItem == null ? NavigationView.TwoPaneViewPriority.Pane1 : NavigationView.TwoPaneViewPriority.Pane2;
            }

            // Dual pane:
            else
            {
                ViewState = ListDetailsViewState.Both;
            }

            if (previousState != ViewState)
            {
                ViewStateChanged?.Invoke(this, ViewState);
                SetBackButtonVisibility(previousState);
            }
        }

        private void SetVisualState(bool animate)
        {
            string noSelectionState;
            string hasSelectionState;
            if (ViewState == ListDetailsViewState.Both)
            {
                noSelectionState = NoSelectionWideState;
                hasSelectionState = HasSelectionWideState;
            }
            else
            {
                noSelectionState = NoSelectionNarrowState;
                hasSelectionState = HasSelectionNarrowState;
            }

            VisualStateManager.GoToState(this, SelectedItem == null ? noSelectionState : hasSelectionState, animate);
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
                if (_detailsPresenter.ContentTemplateSelector != null)
                {
                    _detailsPresenter.ContentTemplate = _detailsPresenter.ContentTemplateSelector.SelectTemplate(SelectedItem, _detailsPresenter);
                }

                // Update the content:
                _detailsPresenter.Content = MapDetails == null
                    ? SelectedItem
                    : SelectedItem != null ? MapDetails(SelectedItem) : null;
            }
        }

        private void OnListCommandBarChanged()
        {
            OnCommandBarChanged("ListCommandBar", ListCommandBar);
        }

        private void OnDetailsCommandBarChanged()
        {
            OnCommandBarChanged("DetailsCommandBar", DetailsCommandBar);
        }

        private void OnCommandBarChanged(string panelName, CommandBar commandbar)
        {
            var panel = GetTemplateChild(panelName) as Panel;
            if (panel == null)
            {
                return;
            }

            panel.Children.Clear();
            if (commandbar != null)
            {
                panel.Children.Add(commandbar);
            }
        }

        /// <summary>
        /// Sets whether the selected item should change when focused with the keyboard based on the view state
        /// </summary>
        /// <param name="viewState">the view state</param>
        private void SetListSelectionWithKeyboardFocusOnVisualStateChanged(ListDetailsViewState viewState)
        {
            if (viewState == ListDetailsViewState.Both)
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
            if (GetTemplateChild("List") is ListViewBase list)
            {
                list.SingleSelectionFollowsFocus = singleSelectionFollowsFocus;
            }
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

        /// <summary>
        /// Sets focus to the relevant control based on the viewState.
        /// </summary>
        /// <param name="viewState">the view state</param>
        private void SetFocus(ListDetailsViewState viewState)
        {
            if (viewState != ListDetailsViewState.Details)
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
            if (GetTemplateChild(PartDetailsPanel) is DependencyObject details)
            {
                var focusableElement = FocusManager.FindFirstFocusableElement(details);
                (focusableElement as Control)?.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Sets focus to the item list
        /// </summary>
        private void FocusItemList()
        {
            if (GetTemplateChild(PartMainList) is Control list)
            {
                list.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Fired when the <see cref="TwoPaneView"/> changed its view mode.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The event args.</param>
        private void OnModeChanged(NavigationView.TwoPaneView sender, object args)
        {
            UpdateView(true);
            SetListSelectionWithKeyboardFocusOnVisualStateChanged(ViewState);
        }

        /// <summary>
        /// Invoked once the items changed and ensures the visual state is constant.
        /// </summary>
        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            UpdateView(true);

            if (SelectedIndex < 0)
            {
                return;
            }

            // Ensure we still have the correct index and selected item for the new collection.
            // This prevents flickering when the order of the collection changes.
            int index = -1;
            if (!(Items is null))
            {
                index = Items.IndexOf(SelectedItem);
            }

            if (index < 0)
            {
                ClearSelectedItem();
            }
            else if (SelectedIndex != index)
            {
                SetValue(SelectedIndexProperty, index);
            }
        }

        /// <summary>
        /// Updates the <see cref="TwoPaneView.Pane1Length"/> since it is of type 'GridLength',
        /// but <see cref="ListPaneWidth"/> is of type <see cref="double"/>.
        /// This should be changed in a further release.
        /// </summary>
        private void OnListPaneWidthChanged()
        {
            if (_twoPaneView != null)
            {
                _twoPaneView.Pane1Length = new GridLength(ListPaneWidth);
            }
        }
    }
}
