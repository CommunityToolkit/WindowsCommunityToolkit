using System;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A class used by the <see cref="SpaceView"/> ItemClicked Event
    /// </summary>
    public class SpaceViewItemClickedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceViewItemClickedEventArgs"/> class.
        /// </summary>
        /// <param name="container">element clicked</param>
        /// <param name="item">data context of element clicked</param>
        public SpaceViewItemClickedEventArgs(ContentControl container, object item)
        {
            Container = container;
            Item = item;
        }

        /// <summary>
        /// Gets or sets the container of the clicked item
        /// </summary>
        public ContentControl Container { get; set; }

        /// <summary>
        /// Gets or sets the Item/Data Context of the clicked item
        /// </summary>
        public object Item { get; set; }
    }
}
