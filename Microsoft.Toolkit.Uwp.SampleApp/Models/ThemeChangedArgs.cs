using System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.Models
{
    public class ThemeChangedArgs : EventArgs
    {
        /// <summary>
        /// Gets the Current UI Theme
        /// </summary>
        public ElementTheme Theme { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the Theme was set by the User.
        /// </summary>
        public bool CustomSet { get; internal set; }
    }
}