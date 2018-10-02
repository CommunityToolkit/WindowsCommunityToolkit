using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Exposes <see cref="RadialGauge"/> to Microsoft UI Automation.
    /// </summary>
    public class RadialGaugeAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGaugeAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The owner element to create for.</param>
        public RadialGaugeAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
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
            return gauge.Value + gauge.Unit + "radial gauge";
        }

        /// <inheritdoc/>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }
    }
}
