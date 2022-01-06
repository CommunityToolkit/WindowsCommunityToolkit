// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;

namespace Microsoft.Toolkit.Uwp.UI.Automation.Peers
{
    /// <summary>
    /// Defines a framework element automation peer for the <see cref="ContentSizer"/> control.
    /// </summary>
    public class ContentSizerAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentSizerAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">
        /// The <see cref="ContentSizer" /> that is associated with this <see cref="T:Windows.UI.Xaml.Automation.Peers.ContentSizerAutomationPeer" />.
        /// </param>
        public ContentSizerAutomationPeer(ContentSizer owner)
            : base(owner)
        {
        }

        private ContentSizer OwningContentSizer
        {
            get
            {
                return Owner as ContentSizer;
            }
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
        /// - Name of the owning ContentSizer
        /// - ContentSizer class name
        /// </returns>
        protected override string GetNameCore()
        {
            string name = AutomationProperties.GetName(this.OwningContentSizer);
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            name = this.OwningContentSizer.Name;
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            name = base.GetNameCore();
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }

            return string.Empty;
        }
    }
}
