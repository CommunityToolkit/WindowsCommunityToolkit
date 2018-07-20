// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Media.Animation.TransitionCollection"/>
    /// </summary>
    public class TransitionCollection
    {
        internal global::Windows.UI.Xaml.Media.Animation.TransitionCollection UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionCollection"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Media.Animation.TransitionCollection"/>
        /// </summary>
        public TransitionCollection(global::Windows.UI.Xaml.Media.Animation.TransitionCollection instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Media.Animation.TransitionCollection"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.TransitionCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.Animation.TransitionCollection"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TransitionCollection(
            global::Windows.UI.Xaml.Media.Animation.TransitionCollection args)
        {
            return FromTransitionCollection(args);
        }

        /// <summary>
        /// Creates a <see cref="TransitionCollection"/> from <see cref="global::Windows.UI.Xaml.Media.Animation.TransitionCollection"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.Animation.TransitionCollection"/> instance containing the event data.</param>
        /// <returns><see cref="TransitionCollection"/></returns>
        public static TransitionCollection FromTransitionCollection(global::Windows.UI.Xaml.Media.Animation.TransitionCollection args)
        {
            return new TransitionCollection(args);
        }
    }
}