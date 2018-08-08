// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Media.Media3D.Transform3D"/>
    /// </summary>
    public class Transform3D
    {
        internal Windows.UI.Xaml.Media.Media3D.Transform3D UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transform3D"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Media.Media3D.Transform3D"/>
        /// </summary>
        public Transform3D(Windows.UI.Xaml.Media.Media3D.Transform3D instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Media.Media3D.Transform3D"/> to <see cref="Transform3D"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.Media3D.Transform3D"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Transform3D(
            Windows.UI.Xaml.Media.Media3D.Transform3D args)
        {
            return FromTransform3D(args);
        }

        /// <summary>
        /// Creates a <see cref="Transform3D"/> from <see cref="Windows.UI.Xaml.Media.Media3D.Transform3D"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Media.Media3D.Transform3D"/> instance containing the event data.</param>
        /// <returns><see cref="Transform3D"/></returns>
        public static Transform3D FromTransform3D(Windows.UI.Xaml.Media.Media3D.Transform3D args)
        {
            return new Transform3D(args);
        }
    }
}