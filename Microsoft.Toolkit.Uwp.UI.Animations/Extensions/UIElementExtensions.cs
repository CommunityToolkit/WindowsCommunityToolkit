// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Extensions
{
    /// <summary>
    /// An extension <see langword="class"/> for the <see cref="UIElement"/> type.
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        /// Returns the desired <see cref="Transform"/> instance after assigning it to the <see cref="UIElement.RenderTransform"/> property of the target <see cref="UIElement"/>.
        /// </summary>
        /// <typeparam name="T">The desired <see cref="Transform"/> type.</typeparam>
        /// <param name="element">The target <see cref="UIElement"/> to modify.</param>
        /// <returns>A <typeparamref name="T"/> instance assigned to <paramref name="element"/>.</returns>
        public static T GetTransform<T>(this UIElement element)
            where T : Transform, new()
        {
            if (element.RenderTransform is T transform)
            {
                return transform;
            }

            return (T)(element.RenderTransform = new T());
        }

        /// <summary>
        /// Returns the <see cref="Visual"/> object for a given <see cref="UIElement"/> instance.
        /// </summary>
        /// <param name="element">The source element to get the visual for.</param>
        /// <returns>The <see cref="Visual"/> object associated with <paramref name="element"/>.</returns>
        [Pure]
        public static Visual GetVisual(this UIElement element)
        {
            return ElementCompositionPreview.GetElementVisual(element);
        }
    }
}
