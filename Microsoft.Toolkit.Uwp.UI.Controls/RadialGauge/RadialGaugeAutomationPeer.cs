// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Exposes <see cref="RadialGauge"/> to Microsoft UI Automation.
    /// </summary>
    public class RadialGaugeAutomationPeer :
        FrameworkElementAutomationPeer,
        IRangeValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGaugeAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The owner element to create for.</param>
        public RadialGaugeAutomationPeer(RadialGauge owner)
            : base(owner)
        {
        }

        /// <inheritdoc/>
        public bool IsReadOnly => !((RadialGauge)Owner).IsInteractive;

        /// <inheritdoc/>
        public double LargeChange => ((RadialGauge)Owner).StepSize;

        /// <inheritdoc/>
        public double Maximum => ((RadialGauge)Owner).Maximum;

        /// <inheritdoc/>
        public double Minimum => ((RadialGauge)Owner).Minimum;

        /// <inheritdoc/>
        public double SmallChange => ((RadialGauge)Owner).StepSize;

        /// <inheritdoc/>
        public double Value => ((RadialGauge)Owner).Value;

        /// <inheritdoc/>
        public void SetValue(double value)
        {
            ((RadialGauge)Owner).Value = value;
        }

        /// <inheritdoc/>
        protected override IList<AutomationPeer> GetChildrenCore()
        {
            return null;
        }

        /// <inheritdoc/>
        protected override string GetNameCore()
        {
            var gauge = (RadialGauge)Owner;
            return "radial gauge. " + (string.IsNullOrWhiteSpace(gauge.Unit) ? "no unit specified. " : "unit " + gauge.Unit + ". ");
        }

        /// <inheritdoc/>
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.RangeValue)
            {
                // Expose RangeValue properties.
                return this;
            }

            return base.GetPatternCore(patternInterface);
        }

        /// <inheritdoc/>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }
    }
}
