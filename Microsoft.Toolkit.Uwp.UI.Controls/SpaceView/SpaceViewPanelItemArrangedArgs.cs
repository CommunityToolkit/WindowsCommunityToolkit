using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event args used by the <see cref="SpaceViewPanel"/> ItemArranged event
    /// </summary>
    public class SpaceViewPanelItemArrangedArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="ElementProperties"/> or arranged item
        /// </summary>
        public SpaceViewElementProperties ElementProperties { get; set; }

        /// <summary>
        /// Gets or sets the index of the item that was arranged
        /// </summary>
        public int ItemIndex { get; set; }
    }
}