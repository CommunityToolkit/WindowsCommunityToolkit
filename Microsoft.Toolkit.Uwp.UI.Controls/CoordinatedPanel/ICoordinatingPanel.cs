// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Interface for a <see cref="Panel"/> to be used as a base for a <see cref="CoordinatablePanel"/>.
    /// </summary>
    public interface ICoordinatingPanel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the panel is in coordinating mode.
        /// </summary>
        bool IsCoordinating { get; set; }

        /// <summary>
        /// Called by child <see cref="InsetPanel"/> to register themselves for layout coordination.
        /// </summary>
        /// <param name="insetPanel"><see cref="InsetPanel"/> to coordinate with.</param>
        void RegisterCoordinatedChild(InsetPanel insetPanel);

        /// <summary>
        /// Provides an abstracted version of <see cref="FrameworkElement.MeasureOverride"/> for use by parent coordinated panel.
        /// </summary>
        /// <param name="availableSize">Size available to the control.</param>
        /// <param name="elements">Collection of elements to measure.</param>
        /// <returns>Desired size.</returns>
        Size MeasureElements(Size availableSize, IEnumerable<UIElement> elements);

        /// <summary>
        /// Provides an abstracted version of <see cref="FrameworkElement.ArrangeOverride"/> for use by parent coordinated panel.
        /// </summary>
        /// <param name="finalSize">Final size available for layout.</param>
        /// <param name="elements">Collection of elements to arrange.</param>
        /// <returns>Final size.</returns>
        Size ArrangeElements(Size finalSize, IEnumerable<UIElement> elements);
    }
}
