// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;

[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.Collapse()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.Expand()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.ExpandCollapseState", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.IScrollItemProvider.ScrollIntoView()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.AddToSelection()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.IsSelected", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.RemoveFromSelection()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.Select()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.TreeViewItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.SelectionContainer", Justification = "Required for subset compat with WPF")]

namespace System.Windows.Automation.Peers
{
    /// <summary>
    /// Exposes the items in
    /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> types to UI
    /// automation.
    /// </summary>
    /// <QualityBand>Stable</QualityBand>
    public partial class TreeViewItemAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, ISelectionItemProvider, IScrollItemProvider
    {
        /// <summary>
        /// Gets the TreeViewItem that owns this TreeViewItemAutomationPeer.
        /// </summary>
        private TreeViewItem OwnerTreeViewItem => (TreeViewItem)Owner;

        /// <summary>
        /// Gets the state (expanded or collapsed) of the control.
        /// </summary>
        /// <value>
        /// The state (expanded or collapsed) of the control.
        /// </value>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
        {
            get
            {
                TreeViewItem owner = OwnerTreeViewItem;
                if (!owner.HasItems)
                {
                    return ExpandCollapseState.LeafNode;
                }
                else if (!owner.IsExpanded)
                {
                    return ExpandCollapseState.Collapsed;
                }
                else
                {
                    return ExpandCollapseState.Expanded;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether an item is selected.
        /// </summary>
        /// <value>True if an item is selected; otherwise, false.</value>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        bool ISelectionItemProvider.IsSelected
        {
            get { return OwnerTreeViewItem.IsSelected; }
        }

        /// <summary>
        /// Gets the UI automation provider that implements
        /// <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" />
        /// and acts as the container for the calling object.
        /// </summary>
        /// <value>The UI automation provider.</value>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
        {
            get
            {
                ItemsControl parent = OwnerTreeViewItem.ParentItemsControl;
                if (parent != null)
                {
                    AutomationPeer peer = FromElement(parent);
                    if (peer != null)
                    {
                        return ProviderFromPeer(peer);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeViewItemAutomationPeer"/>
        /// class.
        /// </summary>
        /// <param name="owner">
        /// The <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> instance
        /// to associate with this
        /// <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />.
        /// </param>
        public TreeViewItemAutomationPeer(TreeViewItem owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Gets the control type for the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> that is
        /// associated with this
        /// <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />.
        /// This method is called by
        /// <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.
        /// </summary>
        /// <returns>
        /// The
        /// <see cref="F:System.Windows.Automation.Peers.AutomationControlType.TreeItem" />
        /// enumeration value.
        /// </returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.TreeItem;
        }

        /// <summary>
        /// Gets the name of the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> that is
        /// associated with this
        /// <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />.
        /// This method is called by
        /// <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.
        /// </summary>
        /// <returns>A string that contains TreeViewItem.</returns>
        protected override string GetClassNameCore()
        {
            return "TreeViewItem";
        }

        /// <summary>
        /// Gets the control pattern for the
        /// <see cref="T:Microsoft.Toolkit.Uwp.UI.Controls.TreeViewItem" /> that is
        /// associated with this
        /// <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" />.
        /// </summary>
        /// <param name="patternInterface">
        /// One of the enumeration values.
        /// </param>
        /// <returns>
        /// The object that implements the pattern interface, or null if the
        /// specified pattern interface is not implemented by this peer.
        /// </returns>
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.ExpandCollapse ||
                patternInterface == PatternInterface.SelectionItem ||
                patternInterface == PatternInterface.ScrollItem)
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Raise the IsSelected property changed event.
        /// </summary>
        /// <param name="isSelected">
        /// A value indicating whether the TreeViewItem is selected.
        /// </param>
        internal void RaiseAutomationIsSelectedChanged(bool isSelected)
        {
            RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, !isSelected, isSelected);
        }

        /// <summary>
        /// Raise an automation event when a TreeViewItem is expanded or
        /// collapsed.
        /// </summary>
        /// <param name="oldValue">
        /// A value indicating whether the TreeViewItem was expanded.
        /// </param>
        /// <param name="newValue">
        /// A value indicating whether the TreeViewItem is expanded.
        /// </param>
        internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            RaisePropertyChangedEvent(
                ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed,
                newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }

        /// <summary>
        /// Displays all child nodes, controls, or content of the control.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        void IExpandCollapseProvider.Expand()
        {
            if (!IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            TreeViewItem owner = OwnerTreeViewItem;
            if (!owner.HasItems)
            {
                throw new InvalidOperationException("Controls.Properties.Resources.Automation_OperationCannotBePerformed");
            }

            owner.IsExpanded = true;
        }

        /// <summary>
        /// Hides all nodes, controls, or content that are descendants of the
        /// control.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        void IExpandCollapseProvider.Collapse()
        {
            if (!IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            TreeViewItem owner = OwnerTreeViewItem;
            if (!owner.HasItems)
            {
                throw new InvalidOperationException("Controls.Properties.Resources.Automation_OperationCannotBePerformed");
            }

            owner.IsExpanded = false;
        }

        /// <summary>
        /// Adds the current element to the collection of selected items.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.AddToSelection()
        {
            TreeViewItem owner = OwnerTreeViewItem;
            TreeView parent = owner.ParentTreeView;
            if (parent == null || (parent.SelectedItem != null && parent.SelectedContainer != Owner))
            {
                throw new InvalidOperationException("Controls.Properties.Resources.Automation_OperationCannotBePerformed");
            }

            owner.IsSelected = true;
        }

        /// <summary>
        /// Clears any selection and then selects the current element.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.Select()
        {
            OwnerTreeViewItem.IsSelected = true;
        }

        /// <summary>
        /// Removes the current element from the collection of selected items.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.RemoveFromSelection()
        {
            OwnerTreeViewItem.IsSelected = false;
        }

        /// <summary>
        /// Scrolls the content area of a container object in order to display
        /// the control within the visible region (viewport) of the container.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not
        /// intended to be used directly from your code.
        /// </remarks>
        void IScrollItemProvider.ScrollIntoView()
        {
            // Note: WPF just calls BringIntoView on the current TreeViewItem.
            // This actually raises an event that can be handled by the
            // its containers.  Silverlight doesn't support this, so we will
            // approximate by moving scrolling the TreeView's ScrollHost to the
            // item.

            // Get the parent TreeView
            TreeViewItem owner = OwnerTreeViewItem;
            TreeView parent = owner.ParentTreeView;
            if (parent == null)
            {
                return;
            }

            // Scroll the item into view
            parent.ItemsControlHelper.ScrollIntoView(owner);
        }
    }
}