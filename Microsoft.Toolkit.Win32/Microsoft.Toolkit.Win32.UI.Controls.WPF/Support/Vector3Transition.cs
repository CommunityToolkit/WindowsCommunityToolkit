// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Vector3Transition"/>
    /// </summary>
    public class Vector3Transition
    {
        internal global::Windows.UI.Xaml.Vector3Transition UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3Transition"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Vector3Transition"/>
        /// </summary>
        public Vector3Transition(global::Windows.UI.Xaml.Vector3Transition instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Vector3Transition.Duration"/>
        /// </summary>
        public System.TimeSpan Duration
        {
            get => UwpInstance.Duration;
            set => UwpInstance.Duration = value;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Vector3Transition.Components"/>
        /// </summary>
        public global::Windows.UI.Xaml.Vector3TransitionComponents Components
        {
            get => UwpInstance.Components;
            set => UwpInstance.Components = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Vector3Transition"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.Vector3Transition"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Vector3Transition"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector3Transition(
            global::Windows.UI.Xaml.Vector3Transition args)
        {
            return FromVector3Transition(args);
        }

        /// <summary>
        /// Creates a <see cref="Vector3Transition"/> from <see cref="global::Windows.UI.Xaml.Vector3Transition"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Vector3Transition"/> instance containing the event data.</param>
        /// <returns><see cref="Vector3Transition"/></returns>
        public static Vector3Transition FromVector3Transition(global::Windows.UI.Xaml.Vector3Transition args)
        {
            return new Vector3Transition(args);
        }
    }
}