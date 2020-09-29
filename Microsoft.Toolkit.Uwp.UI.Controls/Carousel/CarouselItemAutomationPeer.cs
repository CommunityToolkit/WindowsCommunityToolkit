// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace Microsoft.Toolkit.Uwp.UI.Automation.Peers
{
    /// <summary>
    /// Defines a framework element automation peer for the <see cref="CarouselItem"/>.
    /// </summary>
    public class CarouselItemAutomationPeer : FrameworkElementAutomationPeer, ISelectionItemProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarouselItemAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">
        /// The <see cref="CarouselItem" /> that is associated with this <see cref="T:Windows.UI.Xaml.Automation.Peers.CarouselItemAutomationPeer" />.
        /// </param>
        public CarouselItemAutomationPeer(CarouselItem owner)
            : base(owner)
        {
        }

        /// <summary>Gets a value indicating whether an item is selected.</summary>
        /// <returns>True if the element is selected; otherwise, false.</returns>
        public bool IsSelected => this.OwnerCarouselItem.IsSelected;

        /// <summary>Gets the UI Automation provider that implements ISelectionProvider and acts as the container for the calling object.</summary>
        /// <returns>The UI Automation provider.</returns>
        public IRawElementProviderSimple SelectionContainer
        {
            get
            {
                Carousel parent = this.OwnerCarouselItem.ParentCarousel;
                if (parent == null)
                {
                    return null;
                }

                AutomationPeer peer = FromElement(parent);
                return peer != null ? this.ProviderFromPeer(peer) : null;
            }
        }

        private CarouselItem OwnerCarouselItem
        {
            get { return this.Owner as CarouselItem; }
        }

        /// <summary>Adds the current element to the collection of selected items.</summary>
        public void AddToSelection()
        {
            CarouselItem owner = this.OwnerCarouselItem;
            Carousel parent = owner.ParentCarousel;
            parent.SetSelectedItem(owner);
        }

        /// <summary>Removes the current element from the collection of selected items.</summary>
        public void RemoveFromSelection()
        {
            CarouselItem owner = this.OwnerCarouselItem;
            Carousel parent = owner.ParentCarousel;
            parent.SelectedItem = null;
        }

        /// <summary>Clears any existing selection and then selects the current element.</summary>
        public void Select()
        {
            CarouselItem owner = this.OwnerCarouselItem;
            Carousel parent = owner.ParentCarousel;
            parent.SetSelectedItem(owner);
        }

        /// <summary>
        /// Gets the control type for the element that is associated with the UI Automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ListItem;
        }

        /// <summary>
        /// Called by GetClassName that gets a human readable name that, in addition to AutomationControlType,
        /// differentiates the control represented by this AutomationPeer.
        /// </summary>
        /// <returns>The string that contains the name.</returns>
        protected override string GetClassNameCore()
        {
            return Owner.GetType().Name;
        }

        /// <summary>
        /// Called by GetName.
        /// </summary>
        /// <returns>
        /// Returns the first of these that is not null or empty:
        /// - Value returned by the base implementation
        /// - Name of the owning CarouselItem
        /// - Carousel class name
        /// </returns>
        protected override string GetNameCore()
        {
            int? index = this.OwnerCarouselItem.ParentCarousel.GetCarouselItems().ToList().IndexOf(this.OwnerCarouselItem);

            string name = base.GetNameCore();
            if (!string.IsNullOrEmpty(name))
            {
                return $"{name} {index}";
            }

            if (this.OwnerCarouselItem != null && !string.IsNullOrEmpty(this.OwnerCarouselItem.Name))
            {
                return this.OwnerCarouselItem.Name;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = this.GetClassName();
            }

            return $"{name} {index}";
        }

        /// <summary>
        /// Gets the control pattern that is associated with the specified Windows.UI.Xaml.Automation.Peers.PatternInterface.
        /// </summary>
        /// <param name="patternInterface">A value from the Windows.UI.Xaml.Automation.Peers.PatternInterface enumeration.</param>
        /// <returns>The object that supports the specified pattern, or null if unsupported.</returns>
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.SelectionItem:
                    return this;
            }

            return base.GetPatternCore(patternInterface);
        }
    }
}