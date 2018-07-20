// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Media.Brush"/>
    /// </summary>
    public class Brush
    {
        internal global::Windows.UI.Xaml.Media.Brush UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Brush"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Media.Brush"/>
        /// </summary>
        public Brush(global::Windows.UI.Xaml.Media.Brush instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Media.Brush.Transform"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.Transform Transform
        {
            get => UwpInstance.Transform;
            set => UwpInstance.Transform = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Media.Brush.RelativeTransform"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.Transform RelativeTransform
        {
            get => UwpInstance.RelativeTransform;
            set => UwpInstance.RelativeTransform = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Media.Brush.Opacity"/>
        /// </summary>
        public double Opacity
        {
            get => UwpInstance.Opacity;
            set => UwpInstance.Opacity = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="global::Windows.UI.Xaml.Media.Brush"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.Brush"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.Brush"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Brush(
            global::Windows.UI.Xaml.Media.Brush args)
        {
            return FromBrush(args);
        }

        /// <summary>
        /// Creates a <see cref="Brush"/> from <see cref="global::Windows.UI.Xaml.Media.Brush"/>.
        /// </summary>
        /// <param name="args">The <see cref="global::Windows.UI.Xaml.Media.Brush"/> instance containing the event data.</param>
        /// <returns><see cref="Brush"/></returns>
        public static Brush FromBrush(global::Windows.UI.Xaml.Media.Brush args)
        {
            return new Brush(args);
        }
    }
}