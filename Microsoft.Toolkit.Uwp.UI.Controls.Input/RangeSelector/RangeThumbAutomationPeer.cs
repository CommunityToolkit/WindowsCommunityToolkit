// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A class that provides a Microsoft UI Automation peer implementation for <see cref="RangeThumb"/>.
    /// </summary>
    public class RangeThumbAutomationPeer : RangeBaseAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeThumbAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The owner element to create for.</param>
        public RangeThumbAutomationPeer(RangeThumb owner)
            : base(owner)
        {
        }

        /// <inheritdoc/>
        protected override AutomationControlType GetAutomationControlTypeCore() => AutomationControlType.Slider;

        /// <inheritdoc/>
        protected override string GetClassNameCore()
        {
            return nameof(RangeThumb);
        }
    }
}