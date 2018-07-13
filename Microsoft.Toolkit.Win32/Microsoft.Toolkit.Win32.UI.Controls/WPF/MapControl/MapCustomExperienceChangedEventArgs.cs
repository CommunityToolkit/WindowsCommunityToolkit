using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs"/>
    public sealed class MapCustomExperienceChangedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs _args;

        [SecurityCritical]
        internal MapCustomExperienceChangedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs args)
        {
            _args = args;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MapCustomExperienceChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator MapCustomExperienceChangedEventArgs(
            global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs args)
        {
            return FromMapCustomExperienceChangedEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="MapCustomExperienceChangedEventArgs"/> from <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="MapCustomExperienceChangedEventArgs"/></returns>
        public static MapCustomExperienceChangedEventArgs FromMapCustomExperienceChangedEventArgs(global::Windows.UI.Xaml.Controls.Maps.MapCustomExperienceChangedEventArgs args)
        {
            return new MapCustomExperienceChangedEventArgs(args);
        }
    }
}