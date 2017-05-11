using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event args used by the <see cref="SpaceViewPanel"/> ItemsArranged event
    /// </summary>
    public class SpaceViewPanelItemsArrangedArgs
    {
        /// <summary>
        /// Gets or sets a collection of all elements that were arranged.
        /// </summary>
        public List<SpaceViewElementProperties> Elements { get; set; }
    }
}