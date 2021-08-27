// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Interface representing the common properties found within an attached shadow, <see cref="AttachedShadowBase"/> for implementation.
    /// </summary>
    public interface IAttachedShadow
    {
        /// <summary>
        /// Gets or sets the blur radius of the shadow.
        /// </summary>
        double BlurRadius { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the shadow.
        /// </summary>
        double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the offset of the shadow as a string representation of a <see cref="Vector3"/>.
        /// </summary>
        string Offset { get; set; }

        /// <summary>
        /// Gets or sets the color of the shadow.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Get the associated <see cref="AttachedShadowElementContext"/> for the specified <see cref="FrameworkElement"/>.
        /// </summary>
        /// <returns>The <see cref="AttachedShadowElementContext"/> for the element.</returns>
        AttachedShadowElementContext GetElementContext(FrameworkElement element);

        /// <summary>
        /// Gets an enumeration over the current list of <see cref="AttachedShadowElementContext"/> of elements using this shared shadow definition.
        /// </summary>
        /// <returns>Enumeration of <see cref="AttachedShadowElementContext"/> objects.</returns>
        IEnumerable<AttachedShadowElementContext> EnumerateElementContexts();
    }
}
