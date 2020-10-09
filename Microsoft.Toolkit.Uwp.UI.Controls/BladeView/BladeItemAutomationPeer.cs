// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Automation.Peers;

namespace Microsoft.Toolkit.Uwp.UI.Automation.Peers
{
    /// <summary>
    /// Defines a framework element automation peer for the <see cref="BladeItem"/>.
    /// </summary>
    public class BladeItemAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BladeItemAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">
        /// The <see cref="BladeItem" /> that is associated with this <see cref="T:Windows.UI.Xaml.Automation.Peers.BladeItemAutomationPeer" />.
        /// </param>
        public BladeItemAutomationPeer(BladeItem owner)
            : base(owner)
        {
        }

        private BladeItem OwnerBladeItem
        {
            get { return this.Owner as BladeItem; }
        }

        /// <summary>
        /// Gets the control type for the element that is associated with the UI Automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
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
        /// - Name of the owning BladeItem
        /// - BladeItem class name
        /// </returns>
        protected override string GetNameCore()
        {
            int? index = this.OwnerBladeItem.ParentBladeView.GetBladeItems().ToList().IndexOf(this.OwnerBladeItem);

            string name = base.GetNameCore();
            if (!string.IsNullOrEmpty(name))
            {
                return $"{name} {index}";
            }

            if (this.OwnerBladeItem != null && !string.IsNullOrEmpty(this.OwnerBladeItem.Name))
            {
                return this.OwnerBladeItem.Name;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = this.GetClassName();
            }

            return $"{name} {index}";
        }
    }
}