using System;
using System.Security;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Provides data for events. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="global::Windows.UI.Xaml.Navigation.NavigationEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="global::Windows.UI.Xaml.Navigation.NavigationEventArgs"/>
    public sealed class NavigationEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly global::Windows.UI.Xaml.Navigation.NavigationEventArgs _args;

        [SecurityCritical]
        internal NavigationEventArgs(global::Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            _args = args;
        }

        public System.Uri Uri
        {
            [SecurityCritical]
            get => (System.Uri)_args.Uri;
            [SecurityCritical]
            set => _args.Uri = value;
        }

        public object Content
        {
            [SecurityCritical]
            get => (object)_args.Content;
        }

        public global::Windows.UI.Xaml.Navigation.NavigationMode NavigationMode
        {
            [SecurityCritical]
            get => (global::Windows.UI.Xaml.Navigation.NavigationMode)_args.NavigationMode;
        }

        public object Parameter
        {
            [SecurityCritical]
            get => (object)_args.Parameter;
        }

        public System.Type SourcePageType
        {
            [SecurityCritical]
            get => (System.Type)_args.SourcePageType;
        }

        public global::Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo NavigationTransitionInfo
        {
            [SecurityCritical]
            get => (global::Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo)_args.NavigationTransitionInfo;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Navigation.NavigationEventArgs"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.NavigationEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator NavigationEventArgs(
            global::Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            return FromNavigationEventArgs(args);
        }

        /// <summary>
        /// Creates a <see cref="NavigationEventArgs"/> from <see cref="global::Windows.UI.Xaml.Navigation.NavigationEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        /// <returns><see cref="NavigationEventArgs"/></returns>
        public static NavigationEventArgs FromNavigationEventArgs(global::Windows.UI.Xaml.Navigation.NavigationEventArgs args)
        {
            return new NavigationEventArgs(args);
        }
    }
}