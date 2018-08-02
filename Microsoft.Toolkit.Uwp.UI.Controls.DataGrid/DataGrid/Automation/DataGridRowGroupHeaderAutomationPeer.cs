// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Automation.Peers;

namespace Microsoft.Toolkit.Uwp.UI.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for DataGridRowGroupHeader
    /// </summary>
    public class DataGridRowGroupHeaderAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridRowGroupHeaderAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">DataGridRowGroupHeader</param>
        public DataGridRowGroupHeaderAutomationPeer(DataGridRowGroupHeader owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Gets the control type for the element that is associated with the UI Automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Group;
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
    }
}
