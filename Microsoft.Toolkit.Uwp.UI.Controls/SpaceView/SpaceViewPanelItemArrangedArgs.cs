using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class SpaceViewPanelItemArrangedArgs : EventArgs
    {
        public SpaceViewElementProperties ElementProperties { get; set; }

        public int ItemIndex { get; set; }
    }
}