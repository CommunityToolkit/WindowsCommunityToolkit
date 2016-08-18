using System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The Blade is used as a child in the BladeControl
    /// </summary>
    public partial class Blade
    {
        /// <summary>
        /// Fires when the blade is opened or closed
        /// </summary>
        public event EventHandler<Visibility> VisibilityChanged;
    }
}
