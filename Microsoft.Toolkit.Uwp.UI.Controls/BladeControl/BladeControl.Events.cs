using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="Blade"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeControl
    {
        /// <summary>
        /// Fires whenever a <see cref="Blade"/> is opened
        /// </summary>
        public static event EventHandler<Blade> BladeOpened;

        /// <summary>
        /// Fires whenever a <see cref="Blade"/> is closed
        /// </summary>
        public static event EventHandler<Blade> BladeClosed;
    }
}
