// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.Maps.MapScene"/>
    /// </summary>
    public class MapScene
    {
        internal Windows.UI.Xaml.Controls.Maps.MapScene UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapScene"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.Maps.MapScene"/>
        /// </summary>
        public MapScene(Windows.UI.Xaml.Controls.Maps.MapScene instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.Maps.MapScene.TargetCamera"/>
        /// </summary>
        public MapCamera TargetCamera
        {
            get => UwpInstance.TargetCamera;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.Maps.MapScene"/> to <see cref="MapScene"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapScene"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MapScene(
            Windows.UI.Xaml.Controls.Maps.MapScene args)
        {
            return FromMapScene(args);
        }

        /// <summary>
        /// Creates a <see cref="MapScene"/> from <see cref="Windows.UI.Xaml.Controls.Maps.MapScene"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.Maps.MapScene"/> instance containing the event data.</param>
        /// <returns><see cref="MapScene"/></returns>
        public static MapScene FromMapScene(Windows.UI.Xaml.Controls.Maps.MapScene args)
        {
            return new MapScene(args);
        }
    }
}