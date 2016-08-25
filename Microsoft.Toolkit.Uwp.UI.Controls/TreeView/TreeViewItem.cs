// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Automation.Peers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides a selectable item for the
    /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeView" /> control.
    /// </summary>
    /// <QualityBand>Stable</QualityBand>
    [TemplateVisualState(Name = "Expanded", GroupName = "ExpansionStates")]
    [TemplateVisualState(Name = "Collapsed", GroupName = "ExpansionStates")]
    [TemplateVisualState(Name = "HasItems", GroupName = "HasItemsStates")]
    [TemplateVisualState(Name = "NoItems", GroupName = "HasItemsStates")]
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "PointerOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Selected", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "SelectedUnfocused", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "SelectedPointerOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "SelectedPressed", GroupName = "CommonStates")]
    [TemplatePart(Name = ExpanderButtonName, Type = typeof(ToggleButton))]
    [TemplatePart(Name = HeaderName, Type = typeof(FrameworkElement))]
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeViewItem))]
    public partial class TreeViewItem : HeaderedItemsControl, IUpdateVisualState
    {
        /// <summary>
        /// The name of the ExpanderButton template part.
        /// </summary>
        private const string ExpanderButtonName = "ExpanderButton";

        /// <summary>
        /// The name of the Header template part.
        /// </summary>
        private const string HeaderName = "Header";

        /// <summary>
        /// The ExpanderButton template part is used to expand and collapse the
        /// TreeViewItem.
        /// </summary>
        private ToggleButton _expanderButton;

        /// <summary>
        /// Gets or sets the ExpanderButton template part is used to expand and
        /// collapse the TreeViewItem.
        /// </summary>
        private ToggleButton ExpanderButton
        {
            get
            {
                return _expanderButton;
            }

            set
            {
                // Detach from the old ExpanderButton
                if (_expanderButton != null)
                {
                    _expanderButton.Click -= OnExpanderClick;
                    _expanderButton.GotFocus -= OnExpanderGotFocus;
                }

                // Attach to the new ExpanderButton
                _expanderButton = value;
                if (_expanderButton != null)
                {
                    _expanderButton.IsChecked = IsExpanded;
                    _expanderButton.Click += OnExpanderClick;
                    _expanderButton.GotFocus += OnExpanderGotFocus;
                }
            }
        }

        /// <summary>
        /// The Header template part is used to distinguish the bound Header
        /// content of the TreeViewItem.
        /// </summary>
        private FrameworkElement _headerElement;

        /// <summary>
        /// Gets the Header template part that is used to distinguish the bound
        /// Header content of the TreeViewItem.
        /// </summary>
        internal FrameworkElement HeaderElement
        {
            get
            {
                return _headerElement;
            }

            private set
            {
                // Detach from the old Header element
                if (_headerElement != null)
                {
                    _headerElement.PointerPressed -= OnHeaderMouseLeftButtonDown;
                    _headerElement.PointerReleased -= OnHeaderMouseLeftButtonUp;
                }

                // Attach to the new Header element
                _headerElement = value;
                if (_headerElement != null)
                {
                    _headerElement.PointerPressed += OnHeaderMouseLeftButtonDown;
                    _headerElement.PointerReleased += OnHeaderMouseLeftButtonUp;
                }
            }
        }

        /// <summary>
        /// The ExpansionStates visual state group.
        /// </summary>
        private VisualStateGroup _expansionStateGroup;

        /// <summary>
        /// Gets or sets the ExpansionStates visual state group.
        /// </summary>
        private VisualStateGroup ExpansionStateGroup
        {
            get
            {
                return _expansionStateGroup;
            }

            set
            {
                // Detach from the old state group
                if (_expansionStateGroup != null)
                {
                    _expansionStateGroup.CurrentStateChanged -= OnExpansionStateGroupStateChanged;
                }

                // Detach to the new state group
                _expansionStateGroup = value;
                if (_expansionStateGroup != null)
                {
                    _expansionStateGroup.CurrentStateChanged += OnExpansionStateGroupStateChanged;
                }
            }
        }

        /// <summary>
        /// A value indicating whether a read-only dependency property change
        /// handler should allow the value to be set.  This is used to ensure
        /// that read-only properties cannot be changed via SetValue, etc.
        /// </summary>
        private bool _allowWrite;

        /// <summary>
        /// Gets or sets a value indicating whether a dependency property change
        /// handler should ignore the next change notification.  This is used to
        /// reset the value of properties without performing any of the actions
        /// in their change handlers.
        /// </summary>
        internal bool IgnorePropertyChange { get; set; }

        /// <summary>
        /// Gets a value indicating whether this
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> contains
        /// items.
        /// </summary>
        /// <value>
        /// True if this <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />
        /// contains items; otherwise, false. The default is false.
        /// </value>
        public bool HasItems
        {
            get
            {
                return (bool)GetValue(HasItemsProperty);
            }

            private set
            {
                try
                {
                    _allowWrite = true;
                    SetValue(HasItemsProperty, value);
                }
                finally
                {
                    _allowWrite = false;
                }
            }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.HasItems" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.HasItems" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty HasItemsProperty =
            DependencyProperty.Register(
                "HasItems",
                typeof(bool),
                typeof(TreeViewItem),
                new PropertyMetadata(false, OnHasItemsPropertyChanged));

        /// <summary>
        /// HasItemsProperty property changed handler.
        /// </summary>
        /// <param name="d">TreeViewItem that changed its HasItems.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnHasItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem source = d as TreeViewItem;
            if (source == null)
            {
                return;
            }

            // Ignore the change if requested
            if (source.IgnorePropertyChange)
            {
                source.IgnorePropertyChange = false;
                return;
            }

            // Ensure the property is only written when expected
            if (!source._allowWrite)
            {
                // Reset the old value before it was incorrectly written
                source.IgnorePropertyChange = true;
                source.SetValue(HasItemsProperty, e.OldValue);

                throw new InvalidOperationException(
                    "Properties.Resources.TreeViewItem_OnHasItemsPropertyChanged_InvalidWrite");
            }

            source.UpdateVisualState(true);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.ItemsControl.Items" />
        /// contained by this
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> are expanded
        /// or collapsed.
        /// </summary>
        /// <value>
        /// True to indicate the contents of the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.ItemsControl.Items" />
        /// collection are expanded; false to indicate the items are collapsed.
        /// The default is false.
        /// </value>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsExpanded" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsExpanded" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(
                "IsExpanded",
                typeof(bool),
                typeof(TreeViewItem),
                new PropertyMetadata(false, OnIsExpandedPropertyChanged));

        /// <summary>
        /// IsExpandedProperty property changed handler.
        /// </summary>
        /// <param name="d">TreeViewItem that changed its IsExpanded.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem source = d as TreeViewItem;
            if (source == null)
            {
                return;
            }

            bool isExpanded = (bool)e.NewValue;

            // Notify any automation peers of the expansion change
            TreeViewItemAutomationPeer peer = FrameworkElementAutomationPeer.FromElement(source) as TreeViewItemAutomationPeer;
            peer?.RaiseExpandCollapseAutomationEvent((bool)e.OldValue, isExpanded);

            // Raise the Expanded and Collapsed events
            RoutedEventArgs args = new RoutedEventArgs();
            if (isExpanded)
            {
                source.OnExpanded(args);
            }
            else
            {
                source.OnCollapsed(args);
            }

            if (isExpanded)
            {
                // Try to scroll the child TreeViewItems into view once we've
                // finished expanding them.  We do it immediately if there is
                // no ExpansionStateGroup, or wait for the transition to finish
                // if there is one.
                if (source.ExpansionStateGroup == null && source.UserInitiatedExpansion)
                {
                    source.UserInitiatedExpansion = false;

                    TreeView parent = source.ParentTreeView;

                    // Note that we scroll the entire TreeViewItem into view, not just its HeaderElement.
                    parent?.ItemsControlHelper.ScrollIntoView(source);
                }
            }
            else if (source.ContainsSelection)
            {
                // Select the this item if it collapsed the selected item
                source.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> is selected.
        /// </summary>
        /// <value>
        /// True if this <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />
        /// is selected; otherwise, false. The default is false.
        /// </value>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelected" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelected" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                "IsSelected",
                typeof(bool),
                typeof(TreeViewItem),
                new PropertyMetadata(false, OnIsSelectedPropertyChanged));

        /// <summary>
        /// IsSelectedProperty property changed handler.
        /// </summary>
        /// <param name="d">TreeViewItem that changed its IsSelected.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem source = d as TreeViewItem;
            if (source == null)
            {
                return;
            }

            bool isSelected = (bool)e.NewValue;

            // Ignore the change if requested
            if (source.IgnorePropertyChange)
            {
                source.IgnorePropertyChange = false;
                return;
            }

            source.Select(isSelected);

            // Notify any automation peers of the selection change
            TreeViewItemAutomationPeer peer = FrameworkElementAutomationPeer.FromElement(source) as TreeViewItemAutomationPeer;
            peer?.RaiseAutomationIsSelectedChanged(isSelected);

            RoutedEventArgs args = new RoutedEventArgs();
            if (isSelected)
            {
                source.OnSelected(args);
            }
            else
            {
                source.OnUnselected(args);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> has focus.
        /// </summary>
        /// <value>
        /// True if this <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />
        /// has focus; otherwise, false. The default is false.
        /// </value>
        public bool IsSelectionActive
        {
            get
            {
                return (bool)GetValue(IsSelectionActiveProperty);
            }

            private set
            {
                try
                {
                    _allowWrite = true;
                    SetValue(IsSelectionActiveProperty, value);
                }
                finally
                {
                    _allowWrite = false;
                }
            }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelectionActive" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelectionActive" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty IsSelectionActiveProperty =
            DependencyProperty.Register(
                "IsSelectionActive",
                typeof(bool),
                typeof(TreeViewItem),
                new PropertyMetadata(false, OnIsSelectionActivePropertyChanged));

        /// <summary>
        /// IsSelectionActiveProperty property changed handler.
        /// </summary>
        /// <param name="d">TreeViewItem that changed its IsSelectionActive.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsSelectionActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem source = d as TreeViewItem;
            if (source == null)
            {
                return;
            }

            // Ignore the change if requested
            if (source.IgnorePropertyChange)
            {
                source.IgnorePropertyChange = false;
                return;
            }

            // Ensure the property is only written when expected
            if (!source._allowWrite)
            {
                // Reset the old value before it was incorrectly written
                source.IgnorePropertyChange = true;
                source.SetValue(IsSelectionActiveProperty, e.OldValue);

                throw new InvalidOperationException(
                    "Properties.Resources.TreeViewItem_OnIsSelectionActivePropertyChanged_InvalidWrite");
            }

            source.UpdateVisualState(true);
        }

        /// <summary>
        /// Gets the helper that provides all of the standard
        /// interaction functionality.
        /// </summary>
        internal InteractionHelper Interaction { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeView's currently
        /// selected item is a descendent of this TreeViewItem.
        /// </summary>
        private bool ContainsSelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the TreeViewItem should
        /// ignore the next GotFocus event it receives because it has already
        /// been handled by one of its children.
        /// </summary>
        internal bool CancelGotFocusBubble { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether checking ContainsSelection
        /// should actually perform the update notifications because the item
        /// was selected before it was in the visual tree.
        /// </summary>
        internal bool RequiresContainsSelectionUpdate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user initiated action
        /// caused the IsExpanded property to be set.
        /// </summary>
        internal bool UserInitiatedExpansion { get; set; }

        /// <summary>
        /// A reference to the parent ItemsControl of a TreeViewItem.
        /// </summary>
        private ItemsControl _parentItemsControl;

        /// <summary>
        /// Gets or sets a reference to the parent ItemsControl of a
        /// TreeViewItem.
        /// </summary>
        internal ItemsControl ParentItemsControl
        {
            get
            {
                return _parentItemsControl;
            }

            set
            {
                if (_parentItemsControl == value)
                {
                    return;
                }

                _parentItemsControl = value;
                TreeView parentTreeView = ParentTreeView;
                if (parentTreeView != null)
                {
                    // If the selected item was selected while not in the visual
                    // tree, the sequence of ContainsSelection flags are not up
                    // to date.  We'll do that as soon as the SelectedItem is
                    // added to the visual tree.
                    if (RequiresContainsSelectionUpdate)
                    {
                        RequiresContainsSelectionUpdate = false;
                        UpdateContainsSelection(true);
                    }

                    // Ensure the parent TreeView is aware of any selection in
                    // either this TreeViewItem or its descendents.
                    parentTreeView.CheckForSelectedDescendents(this);
                }
            }
        }

        /// <summary>
        /// Gets a reference to the parent TreeViewItem of this TreeViewItem.
        /// </summary>
        internal TreeViewItem ParentTreeViewItem => ParentItemsControl as TreeViewItem;

        /// <summary>
        /// Gets a reference to the parent TreeView of the TreeViewItem.
        /// </summary>
        internal TreeView ParentTreeView
        {
            get
            {
                TreeViewItem current = this;
                while (current != null)
                {
                    TreeView view = current.ParentItemsControl as TreeView;
                    if (view != null)
                    {
                        return view;
                    }

                    current = current.ParentTreeViewItem;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this TreeViewItem is a root of the
        /// TreeView.
        /// </summary>
        private bool IsRoot => ParentItemsControl is TreeView;

        /// <summary>
        /// Gets a value indicating whether the TreeViewItem can expand when it
        /// receives appropriate user input.
        /// </summary>
        private bool CanExpandOnInput => HasItems && IsEnabled;

        /// <summary>
        /// Occurs when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsExpanded" />
        /// property changes from true to false.
        /// </summary>
        public event RoutedEventHandler Collapsed;

        /// <summary>
        /// Occurs when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsExpanded" />
        /// property changes from false to true.
        /// </summary>
        public event RoutedEventHandler Expanded;

        /// <summary>
        /// Occurs when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelected" />
        /// property of a <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />
        /// changes from false to true.
        /// </summary>
        public event RoutedEventHandler Selected;

        /// <summary>
        /// Occurs when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelected" />
        /// property of a <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />
        /// changes from true to false.
        /// </summary>
        public event RoutedEventHandler Unselected;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeViewItem"/> class.
        /// </summary>
        public TreeViewItem()
        {
            DefaultStyleKey = typeof(TreeViewItem);
            Interaction = new InteractionHelper(this);
        }

        /// <summary>
        /// Returns a
        /// <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />
        /// for use by the Silverlight automation infrastructure.
        /// </summary>
        /// <returns>
        /// A
        /// <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />
        /// object for the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />.
        /// </returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TreeViewItemAutomationPeer(this);
        }

        /// <summary>
        /// Builds the visual tree for the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> control when a
        /// new control template is applied.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            //ItemsControlHelper.OnApplyTemplate();

            // Get the template parts
            ExpanderButton = GetTemplateChild(ExpanderButtonName) as ToggleButton;
            HeaderElement = GetTemplateChild(HeaderName) as FrameworkElement;

            // Try to get the ExpansionStates visual state group
            ExpansionStateGroup = VisualStates.TryGetVisualStateGroup(this, "ExpansionStates");

            Interaction.OnApplyTemplateBase();
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Provides handling for the ExpansionStates CurrentChanged event.
        /// </summary>
        /// <param name="sender">The ExpansionState VisualStateGroup.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExpansionStateGroupStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            // Only listen for the Expanded state
            if (string.Compare(e.NewState.Name, "Expanded", StringComparison.OrdinalIgnoreCase) != 0)
            {
                return;
            }

            BringIntoView();
        }

        /// <summary>
        /// Scroll the TreeViewItem into view.
        /// </summary>
        private void BringIntoView()
        {
            // Scroll the TreeViewItem into view when it's expanded
            if (UserInitiatedExpansion)
            {
                UserInitiatedExpansion = false;

                if (ParentTreeView != null)
                {
                    LayoutUpdated += TreeViewItem_LayoutUpdated;
                    // The child items aren't necessarily in the visual tree yet
                    // so we use UpdateLayout() before trying to scroll
                    // so there size will be respected.
                }
            }
        }

        private void TreeViewItem_LayoutUpdated(object sender, object e)
        {
            ParentTreeView.ItemsControlHelper.ScrollIntoView(this);
            LayoutUpdated -= TreeViewItem_LayoutUpdated;
        }

        /// <summary>
        /// Update the visual state of the control.
        /// </summary>
        /// <param name="useTransitions">
        /// A value indicating whether to automatically generate transitions to
        /// the new state, or instantly transition to the new state.
        /// </param>
        void IUpdateVisualState.UpdateVisualState(bool useTransitions)
        {
            UpdateVisualState(useTransitions);
        }

        /// <summary>
        /// Update the visual state of the control.
        /// </summary>
        /// <param name="useTransitions">
        /// A value indicating whether to automatically generate transitions to
        /// the new state, or instantly transition to the new state.
        /// </param>
        internal virtual void UpdateVisualState(bool useTransitions)
        {
            // Handle the Expansion states
            if (IsExpanded)
            {
                VisualStates.GoToState(this, useTransitions, "Expanded");
            }
            else
            {
                VisualStates.GoToState(this, useTransitions, "Collapsed");
            }

            // Handle the HasItems states
            if (HasItems)
            {
                VisualStates.GoToState(this, useTransitions, "HasItems");
            }
            else
            {
                VisualStates.GoToState(this, useTransitions, "NoItems");
            }

            // Handle the Selected states
            if (IsSelected)
            {
                if (Interaction.IsMouseOver)
                {
                    if (Interaction.IsPressed)
                    {
                        VisualStates.GoToState(this, useTransitions, "SelectedPressed");
                    }
                    else
                    {
                        VisualStates.GoToState(this, useTransitions, "SelectedPointerOver");
                    }
                }
                else
                {
                    if (IsSelectionActive)
                    {
                        VisualStates.GoToState(this, useTransitions, "Selected");
                    }
                    else
                    {
                        VisualStates.GoToState(this, useTransitions, "SelectedUnfocused");
                    }
                }
            }
            else
            {
                if (Interaction.IsMouseOver)
                {
                    if (Interaction.IsPressed)
                    {
                        VisualStates.GoToState(this, useTransitions, "Pressed");
                    }
                    else
                    {
                        VisualStates.GoToState(this, useTransitions, "PointerOver");
                    }
                }
                else
                {
                    VisualStates.GoToState(this, useTransitions, "Normal");
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> to
        /// display content.
        /// </summary>
        /// <returns>
        /// A <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> to use as a
        /// container for content.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeViewItem();
        }

        /// <summary>
        /// Determines whether an object is a
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />.
        /// </summary>
        /// <param name="item">The object to evaluate.</param>
        /// <returns>
        /// True if <paramref name="item" /> is a
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />; otherwise,
        /// false.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeViewItem;
        }

        /// <summary>
        /// Prepares the specified container element to display the specified
        /// item.
        /// </summary>
        /// <param name="element">
        /// Container element used to display the specified item.
        /// </param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            TreeViewItem node = element as TreeViewItem;
            if (node != null)
            {
                // Associate the Parent ItemsControl
                node.ParentItemsControl = this;

                string isSelectedBindingPath = TreeView.GetIsSelectedBindingPath(this) ?? TreeView.GetIsSelectedBindingPath(node);
                string isExpandedBindingPath = TreeView.GetIsExpandedBindingPath(this) ?? TreeView.GetIsExpandedBindingPath(node);

                if (isSelectedBindingPath != null)
                {
                    if (node.ReadLocalValue(TreeView.IsSelectedBindingPathProperty) == DependencyProperty.UnsetValue)
                    {
                        TreeView.SetIsSelectedBindingPath(node, isSelectedBindingPath);
                    }

                    node.SetBinding(IsSelectedProperty, new Binding { Path = new PropertyPath(isSelectedBindingPath), Mode = BindingMode.TwoWay });
                }

                if (isExpandedBindingPath != null)
                {
                    if (node.ReadLocalValue(TreeView.IsExpandedBindingPathProperty) == DependencyProperty.UnsetValue)
                    {
                        TreeView.SetIsExpandedBindingPath(node, isExpandedBindingPath);
                    }

                    node.SetBinding(IsExpandedProperty, new Binding { Path = new PropertyPath(isExpandedBindingPath), Mode = BindingMode.TwoWay });
                }
            }

            base.PrepareContainerForItemOverride(element, item);
        }

        /// <summary>
        /// Removes all templates, styles, and bindings for the object displayed
        /// as a <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />.
        /// </summary>
        /// <param name="element">
        /// The <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> element to
        /// clear.
        /// </param>
        /// <param name="item">
        /// The item that is contained in the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" />.
        /// </param>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            TreeViewItem node = element as TreeViewItem;
            if (node != null)
            {
                node.ParentItemsControl = null;
            }

            base.ClearContainerForItemOverride(element, item);
        }

        private List<object> _items;

        /// <summary>
        /// Makes adjustments to the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> when the value
        /// of the <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.ItemsControl.Items" />
        /// property changes.
        /// </summary>
        /// <param name="e">
        /// A
        /// <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" />
        /// that contains data about the change.
        /// </param>
        protected override void OnItemsChanged(object e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            base.OnItemsChanged(e);
            HasItems = Items.Count > 0;

            if (_items == null)
            {
                _items = new List<object>();
            }

            List<object> newItems = Items.Except(_items).ToList();

            // Associate any TreeViewItems with their parent
            if (newItems != null)
            {
                foreach (TreeViewItem item in newItems.OfType<TreeViewItem>())
                {
                    item.ParentItemsControl = this;
                }
            }

            switch (true)
            {
                default:
                    if (!ContainsSelection)
                    {
                        break;
                    }

                    TreeView parentTreeView = ParentTreeView;
                    if (parentTreeView == null || parentTreeView.IsSelectedContainerHookedUp)
                    {
                        break;
                    }

                    ContainsSelection = false;
                    Select(true);
                    break;
            }

            List<object> oldItems = _items.Except(Items).ToList();

            // Remove the association with the Parent ItemsControl
            if (oldItems != null)
            {
                foreach (TreeViewItem item in oldItems.OfType<TreeViewItem>())
                {
                    item.ParentItemsControl = null;
                }
            }

            _items = Items.ToList();
        }

        /// <summary>
        /// Raise a RoutedEvent.
        /// </summary>
        /// <param name="handler">Event handler.</param>
        /// <param name="args">Event arguments.</param>
        private void RaiseEvent(RoutedEventHandler handler, RoutedEventArgs args)
        {
            handler?.Invoke(this, args);
        }

        /// <summary>
        /// Raises an
        /// <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.Expanded" /> event
        /// when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsExpanded" />
        /// property changes from false to true.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.RoutedEventArgs" /> that contains the
        /// event data.
        /// </param>
        protected virtual void OnExpanded(RoutedEventArgs e)
        {
            ToggleExpanded(Expanded, e);
        }

        /// <summary>
        /// Raises a
        /// <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.Collapsed" />
        /// event when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsExpanded" />
        /// property changes from true to false.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.RoutedEventArgs" /> that contains the
        /// event data.
        /// </param>
        protected virtual void OnCollapsed(RoutedEventArgs e)
        {
            ToggleExpanded(Collapsed, e);
        }

        /// <summary>
        /// Handle changes to the IsExpanded property.
        /// </summary>
        /// <param name="handler">Event handler.</param>
        /// <param name="args">Event arguments.</param>
        private void ToggleExpanded(RoutedEventHandler handler, RoutedEventArgs args)
        {
            ToggleButton expander = ExpanderButton;
            if (expander != null)
            {
                expander.IsChecked = IsExpanded;
            }

            UpdateVisualState(true);
            RaiseEvent(handler, args);
        }

        /// <summary>
        /// Raises the
        /// <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.Selected" /> event
        /// when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelected" />
        /// property changes from false to true.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.RoutedEventArgs" /> that contains the
        /// event data.
        /// </param>
        protected virtual void OnSelected(RoutedEventArgs e)
        {
            UpdateVisualState(true);
            RaiseEvent(Selected, e);
        }

        /// <summary>
        /// Raises the
        /// <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.Unselected" />
        /// event when the
        /// <see cref="P:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem.IsSelected" />
        /// property changes from true to false.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.RoutedEventArgs" /> that contains the
        /// event data.
        /// </param>
        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            UpdateVisualState(true);
            RaiseEvent(Unselected, e);
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.GotFocus" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.RoutedEventArgs" /> that contains the
        /// event data.
        /// </param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            // Since the GotFocus event will bubble up to the parent
            // TreeViewItem (which will make it think it's also selected), it
            // needs to ignore that event when it's first been handled by one of
            // its nested children.  We use the IgnoreNextGotFocus flag to
            // notify our parent that GotFocus has already been handled.
            if (ParentTreeViewItem != null)
            {
                ParentTreeViewItem.CancelGotFocusBubble = true;
            }

            try
            {
                if (Interaction.AllowGotFocus(e) && !CancelGotFocusBubble)
                {
                    // Select the item when it's focused
                    Select(true);

                    // ActivateAsync the selection
                    IsSelectionActive = true;
                    UpdateVisualState(true);

                    Interaction.OnGotFocusBase();
                    base.OnGotFocus(e);
                }
            }
            finally
            {
                CancelGotFocusBubble = false;
            }
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.LostFocus" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.RoutedEventArgs" /> that contains the
        /// event data.
        /// </param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (Interaction.AllowLostFocus(e))
            {
                Interaction.OnLostFocusBase();
                base.OnLostFocus(e);
            }

            // Deactivate the selection
            IsSelectionActive = false;
            UpdateVisualState(true);
        }

        /// <summary>
        /// Handle the ExpanderButton's GotFocus event.
        /// </summary>
        /// <param name="sender">The ExpanderButton.</param>
        /// <param name="e">Event Arguments.</param>
        private void OnExpanderGotFocus(object sender, RoutedEventArgs e)
        {
            CancelGotFocusBubble = true;

            // ActivateAsync the selection
            IsSelectionActive = true;
            UpdateVisualState(true);
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.MouseEnter" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains
        /// the event data.
        /// </param>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            if (Interaction.AllowPointerEnter(e))
            {
                Interaction.OnPointerEnterBase();
                base.OnPointerEntered(e);
            }
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.PointerMoved" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.PointerRoutedEventArgs" /> that contains
        /// the event data.
        /// </param>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (Interaction.AllowPointerEnter(e))
            {
                Interaction.OnPointerEnterBase();
                base.OnPointerMoved(e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
            if (Interaction.AllowPointerLeave(e))
            {
                Interaction.OnPointerLeaveBase();
                base.OnPointerCaptureLost(e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPointerCanceled(PointerRoutedEventArgs e)
        {
            base.OnPointerCanceled(e);
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains
        /// the event data.
        /// </param>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (Interaction.AllowPointerLeave(e))
            {
                Interaction.OnPointerLeaveBase();
                base.OnPointerExited(e);
            }
        }

        /// <summary>
        /// Provides handling for the Header's MouseLeftButtonDown event.
        /// </summary>
        /// <param name="sender">The Header template part.</param>
        /// <param name="e">Event arguments.</param>
        private void OnHeaderMouseLeftButtonDown(object sender, PointerRoutedEventArgs e)
        {
            if (Interaction.AllowMouseLeftButtonDown(e))
            {
                // If the event hasn't already been handled and this item is
                // focusable, then focus (and possibly expand if it was double
                // clicked)
                if (!e.Handled && IsEnabled)
                {
                    // Expand the item when double clicked
                    if (Interaction.ClickCount % 2 == 0)
                    {
                        bool opened = !IsExpanded;
                        UserInitiatedExpansion |= opened;
                        IsExpanded = opened;

                        e.Handled = true;
                    }
                }

                Interaction.OnMouseLeftButtonDownBase();
                OnPointerPressed(e);
            }
        }

        private void OnHeaderMouseLeftButtonUp(object sender, PointerRoutedEventArgs e)
        {
            if (Interaction.AllowMouseLeftButtonUp(e))
            {
                // If the event hasn't already been handled and this item is
                // focusable, then focus (and possibly expand if it was double
                // clicked)
                if (!e.Handled && IsEnabled)
                {
                    if (Focus(FocusState.Programmatic))
                    {
                        e.Handled = true;
                    }
                }

                Interaction.OnMouseLeftButtonUpBase();
                OnPointerReleased(e);
            }
        }

        /// <summary>
        /// Provides handling for the ExpanderButton's Click event.
        /// </summary>
        /// <param name="sender">The ExpanderButton.</param>
        /// <param name="e">Event Arguments.</param>
        private void OnExpanderClick(object sender, RoutedEventArgs e)
        {
            bool opened = !IsExpanded;
            UserInitiatedExpansion |= opened;
            IsExpanded = opened;
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that
        /// contains the event data.
        /// </param>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            TreeView parent;
            if (!e.Handled && (parent = ParentTreeView) != null && parent.HandleMouseButtonDown())
            {
                e.Handled = true;
            }

            base.OnPointerPressed(e);
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that
        /// contains the event data.
        /// </param>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.KeyDown" /> event when the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> has focus.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains
        /// the event data.
        /// </param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Complexity metric is inflated by the switch statements")]
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);

            if (Interaction.AllowKeyDown(e))
            {
                if (e.Handled)
                {
                    return;
                }

                // Some keys (e.g. Left/Right) need to be translated in RightToLeft mode
                VirtualKey invariantKey = InteractionHelper.GetLogicalKey(FlowDirection, e.Key);

                // We ignore the Control modifier key because it is used
                // specifically for scrolling which is implemented in the
                // TreeView. We do not mark the event handled if Control is
                // pressed so that it will bubble up to the parent TreeView of
                // this item.
                switch (invariantKey)
                {
                    case VirtualKey.Left:
                        // The Left key will collapse an expanded item or
                        // move to the parent if the item is already
                        // collapsed.
                        if (!TreeView.IsControlKeyDown && CanExpandOnInput && IsExpanded)
                        {
                            if (FocusManager.GetFocusedElement() != this)
                            {
                                Focus(FocusState.Programmatic);
                            }
                            else
                            {
                                IsExpanded = false;
                            }

                            e.Handled = true;
                        }

                        break;
                    case VirtualKey.Right:
                        // The Right key is only useful when the item has
                        // children
                        if (!TreeView.IsControlKeyDown && CanExpandOnInput)
                        {
                            // Expand the item if it is collapsed or move into
                            // the first child if it is already expanded.
                            if (!IsExpanded)
                            {
                                UserInitiatedExpansion = true;
                                IsExpanded = true;
                                e.Handled = true;
                            }
                            else if (HandleDownKey())
                            {
                                e.Handled = true;
                            }
                        }

                        break;
                    case VirtualKey.Up:
                        if (!TreeView.IsControlKeyDown && HandleUpKey())
                        {
                            e.Handled = true;
                        }

                        break;
                    case VirtualKey.Down:
                        if (!TreeView.IsControlKeyDown && HandleDownKey())
                        {
                            e.Handled = true;
                        }

                        break;
                    case VirtualKey.Add:
                        if (CanExpandOnInput && !IsExpanded)
                        {
                            UserInitiatedExpansion = true;
                            IsExpanded = true;
                            e.Handled = true;
                        }

                        break;
                    case VirtualKey.Subtract:
                        if (CanExpandOnInput && IsExpanded)
                        {
                            IsExpanded = false;
                            e.Handled = true;
                        }

                        break;
                }
            }

            // Because Silverlight's ScrollViewer swallows many useful key
            // events (which it can ignore on WPF if you override
            // HandlesScrolling or use an internal only variable in
            // Silverlight), the root TreeViewItems explicitly propagate KeyDown
            // events to their parent TreeView.
            if (IsRoot)
            {
                TreeView parent = ParentTreeView;
                parent?.PropagateKeyDown(e);
            }
        }

        /// <summary>
        /// Try moving the focus down from the selected item.
        /// </summary>
        /// <returns>
        /// A value indicating whether the focus was successfully moved.
        /// </returns>
        internal bool HandleDownKey()
        {
            // Check if the item should handle the key and then try to move down
            return AllowKeyHandleEvent() && FocusDown();
        }

        /// <summary>
        /// Provides handling for the
        /// <see cref="E:System.Windows.UIElement.KeyUp" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains
        /// the event data.
        /// </param>
        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            if (Interaction.AllowKeyUp(e))
            {
                base.OnKeyUp(e);
            }
        }

        /// <summary>
        /// Try moving the focus up from the selected item.
        /// </summary>
        /// <returns>
        /// A value indicating whether the focus was successfully moved.
        /// </returns>
        internal bool HandleUpKey()
        {
            // Check if the item should handle the key
            if (AllowKeyHandleEvent())
            {
                // Get the focusable item directly above this item (note that
                // this is not necessarily a hierarchical sibling - it could
                // also be an ancestor or a descedent of a sibling/ancestor)
                ItemsControl control = FindPreviousFocusableItem();
                if (control != null)
                {
                    // Try to focus the item unless it's the parent TreeView
                    return (control == ParentItemsControl && control == ParentTreeView) || control.Focus(FocusState.Programmatic);
                }
            }

            return false;
        }

        /// <summary>
        /// Handle scrolling a page up or down.
        /// </summary>
        /// <param name="up">
        /// A value indicating whether the page should be scrolled up.
        /// </param>
        /// <param name="scrollHost">The ScrollViewer being scrolled.</param>
        /// <param name="viewportHeight">The height of the viewport.</param>
        /// <param name="top">The top of item to start from.</param>
        /// <param name="bottom">The bottom of the item to start from.</param>
        /// <param name="currentDelta">The height of this item.</param>
        /// <returns>
        /// A value indicating whether the scroll was handled.
        /// </returns>
        internal bool HandleScrollByPage(bool up, ScrollViewer scrollHost, double viewportHeight, double top, double bottom, out double currentDelta)
        {
            double closeEdge;
            currentDelta = CalculateDelta(up, this, scrollHost, top, bottom, out closeEdge);
            if (NumericHelper.IsGreaterThan(closeEdge, viewportHeight))
            {
                // The item doesn't fit in the view
                return false;
            }

            if (NumericHelper.IsLessThanOrClose(currentDelta, viewportHeight))
            {
                // The item and its children fit, but don't focus because there
                // might be more than fit
                return false;
            }
            else
            {
                // The item is partially in the view

                // Check if the header is in the view
                bool headerInView = false;
                if (HeaderElement != null)
                {
                    double edge;
                    double delta = CalculateDelta(up, HeaderElement, scrollHost, top, bottom, out edge);
                    if (NumericHelper.IsLessThanOrClose(delta, viewportHeight))
                    {
                        headerInView = true;
                    }
                }

                TreeViewItem selected = null;
                int count = Items.Count;
                bool skip = up && ContainsSelection;
                for (int index = up ? count - 1 : 0; index >= 0 && index < count; index += up ? -1 : 1)
                {
                    TreeViewItem item = ContainerFromIndex(index) as TreeViewItem;

                    if (item != null && item.IsEnabled)
                    {
                        if (skip)
                        {
                            if (item.IsSelected)
                            {
                                skip = false;
                                continue;
                            }
                            else if (item.ContainsSelection)
                            {
                                skip = false;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        double delta;
                        if (item.HandleScrollByPage(up, scrollHost, viewportHeight, top, bottom, out delta))
                        {
                            // This item or one of its children was focused
                            return true;
                        }
                        else if (NumericHelper.IsGreaterThan(delta, viewportHeight))
                        {
                            // The item does not fit
                            break;
                        }
                        else
                        {
                            // The item fits, but continue searching
                            selected = item;
                        }
                    }
                }

                if (selected != null)
                {
                    if (up)
                    {
                        return selected.Focus(FocusState.Programmatic);
                    }
                    else
                    {
                        return selected.FocusInto();
                    }
                }
                else if (headerInView)
                {
                    // If none of the children fit but the header is in view,
                    // then we'll select this TreeViewItem.
                    return Focus(FocusState.Programmatic);
                }
            }

            return false;
        }

        /// <summary>
        /// Calculate the distance between this TreeViewItem and the item being
        /// paged from.
        /// </summary>
        /// <param name="up">
        /// A value indicating whether the page should be scrolled up.
        /// </param>
        /// <param name="element">The element being paged from.</param>
        /// <param name="scrollHost">The ScrollViewer being scrolled.</param>
        /// <param name="top">The top of item to start from.</param>
        /// <param name="bottom">The bottom of the item to start from.</param>
        /// <param name="closeEdge">
        /// The distance between the top/bottom of one item to the other.
        /// </param>
        /// <returns>
        /// A value indicating whether the scroll was handled.
        /// </returns>
        private static double CalculateDelta(bool up, FrameworkElement element, ScrollViewer scrollHost, double top, double bottom, out double closeEdge)
        {
            double elementTop, elementBottom;
            element.GetTopAndBottom(scrollHost, out elementTop, out elementBottom);

            if (up)
            {
                closeEdge = bottom - elementBottom;
                return bottom - elementTop;
            }
            else
            {
                closeEdge = elementTop - top;
                return elementBottom - top;
            }
        }

        /// <summary>
        /// Change the selected status of the TreeViewItem.
        /// </summary>
        /// <param name="selected">
        /// A value indicating whether the TreeViewItem is selected.
        /// </param>
        private void Select(bool selected)
        {
            // Get the parent TreeView and make sure it's not already in the
            // process of changing the selection
            TreeView view = ParentTreeView;
            if (view != null && !view.IsSelectionChangeActive)
            {
                // Change the selection in the TreeView
                TreeViewItem parent = ParentTreeViewItem;
                object item = parent != null ? parent.ItemFromContainer(this) : view.ItemFromContainer(this);
                view.ChangeSelection(item, this, selected);
            }
        }

        /// <summary>
        /// Update the ancestors of this item when it changes selection.
        /// </summary>
        /// <param name="selected">
        /// A value indicating whether the item is selected.
        /// </param>
        /// <remarks>
        /// Unselection updates need to occur before selection updates because
        /// the old and new selected items may share a partial path.
        /// </remarks>
        internal void UpdateContainsSelection(bool selected)
        {
            // Update the ContainsSelection flag from this item all the way up
            // through the tree.
            for (TreeViewItem item = ParentTreeViewItem; item != null; item = item.ParentTreeViewItem)
            {
                item.ContainsSelection = selected;
            }
        }

        /// <summary>
        /// Determine whether the TreeViewItem should be allowed to handle a key
        /// event.
        /// </summary>
        /// <returns>
        /// A value indicating whether the key event should be handled.
        /// </returns>
        private bool AllowKeyHandleEvent()
        {
            return IsSelected;
        }

        /// <summary>
        /// Navigate the focus to the next TreeViewItem below this item.
        /// </summary>
        /// <returns>
        /// A value indicating whether the focus was navigated.
        /// </returns>
        internal bool FocusDown()
        {
            TreeViewItem item = FindNextFocusableItem(true);
            return item != null && item.Focus(FocusState.Programmatic);
        }

        /// <summary>
        /// Navigate the focus to the very last TreeViewItem descendent of the
        /// this item.
        /// </summary>
        /// <returns>
        /// A value indicating whether the focus was navigated.
        /// </returns>
        internal bool FocusInto()
        {
            TreeViewItem last = FindLastFocusableItem();
            return last != null && last.Focus(FocusState.Programmatic);
        }

        /// <summary>
        /// Find the next focusable TreeViewItem below this item.
        /// </summary>
        /// <param name="recurse">
        /// A value indicating whether the item should recurse into its child
        /// items when searching for the next focusable TreeViewItem.
        /// </param>
        /// <returns>The next focusable TreeViewItem below this item.</returns>
        private TreeViewItem FindNextFocusableItem(bool recurse)
        {
            // Look for the next item in the children of this item (if allowed)
            if (recurse && IsExpanded && HasItems)
            {
                TreeViewItem item = ContainerFromIndex(0) as TreeViewItem;

                if (item != null)
                {
                    return item.IsEnabled ?
                        item :
                        item.FindNextFocusableItem(false);
                }
            }

            // Look for the next item in the siblings of this item
            ItemsControl parent = ParentItemsControl;
            if (parent != null)
            {
                // Get the index of this item relative to its siblings
                int index = parent.IndexFromContainer(this);

                int count = parent.Items.Count;

                // Check for any siblings below this item
                while (index++ < count)
                {
                    TreeViewItem item = parent.ContainerFromIndex(index) as TreeViewItem;

                    if (item != null && item.IsEnabled)
                    {
                        return item;
                    }
                }

                // If nothing else was found, try to find the next sibling below
                // the parent of this item
                TreeViewItem parentItem = ParentTreeViewItem;
                if (parentItem != null)
                {
                    return parentItem.FindNextFocusableItem(false);
                }
            }

            return null;
        }

        /// <summary>
        /// Find the last focusable TreeViewItem contained by this item.
        /// </summary>
        /// <returns>
        /// The last focusable TreeViewItem contained by this item.
        /// </returns>
        private TreeViewItem FindLastFocusableItem()
        {
            TreeViewItem item = this;
            TreeViewItem lastItem = null;
            int index = -1;

            // Walk the children of the current item
            while (item != null)
            {
                // Ignore any disabled items
                if (item.IsEnabled)
                {
                    // If the item has no children, it must be the last
                    if (!item.IsExpanded || !item.HasItems)
                    {
                        return item;
                    }

                    // If the item has children, mark it as the last known
                    // focusable item so far and walk into its child items,
                    // starting from the last item and moving toward the first
                    lastItem = item;
                    index = item.Items.Count - 1;
                }
                else if (index > 0)
                {
                    // Try searching for the previous item's sibling
                    index--;
                }
                else
                {
                    // Stop searching if we've run out of children
                    break;
                }

                // Move to the item's previous sibling
                item = lastItem?.ContainerFromIndex(index) as TreeViewItem;
            }

            return lastItem;
        }

        /// <summary>
        /// Find the previous focusable TreeViewItem above this item.
        /// </summary>
        /// <returns>
        /// The previous focusable TreeViewItem above this item.
        /// </returns>
        private ItemsControl FindPreviousFocusableItem()
        {
            ItemsControl parent = ParentItemsControl;
            if (parent == null)
            {
                return null;
            }

            // Get the index of the current item relative to its siblings
            int index = parent.IndexFromContainer(this);

            // Walk the previous siblings of the item to find a focusable item
            while (index-- > 0)
            {
                // Get the sibling
                TreeViewItem item = parent.ContainerFromIndex(index) as TreeViewItem;

                if (item != null && item.IsEnabled)
                {
                    // Get the last focusable descendent of the sibling
                    TreeViewItem last = item.FindLastFocusableItem();
                    if (last != null)
                    {
                        return last;
                    }
                }
            }

            return parent;
        }
    }
}
