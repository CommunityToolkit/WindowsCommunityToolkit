// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Protected helper methods for <see cref="SizerBase"/> and subclasses.
    /// </summary>
    public partial class SizerBase : Control
    {
        /// <summary>
        /// Check for new requested vertical size is valid or not
        /// </summary>
        /// <param name="target">Target control being resized</param>
        /// <param name="newHeight">The requested new height</param>
        /// <param name="parentActualHeight">The parent control's ActualHeight</param>
        /// <returns>Bool result if requested vertical change is valid or not</returns>
        protected static bool IsValidHeight(FrameworkElement target, double newHeight, double parentActualHeight)
        {
            var minHeight = target.MinHeight;
            if (newHeight < 0 || (!double.IsNaN(minHeight) && newHeight < minHeight))
            {
                return false;
            }

            var maxHeight = target.MaxHeight;
            if (!double.IsNaN(maxHeight) && newHeight > maxHeight)
            {
                return false;
            }

            if (newHeight <= parentActualHeight)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check for new requested horizontal size is valid or not
        /// </summary>
        /// <param name="target">Target control being resized</param>
        /// <param name="newWidth">The requested new width</param>
        /// <param name="parentActualWidth">The parent control's ActualWidth</param>
        /// <returns>Bool result if requested horizontal change is valid or not</returns>
        protected static bool IsValidWidth(FrameworkElement target, double newWidth, double parentActualWidth)
        {
            var minWidth = target.MinWidth;
            if (newWidth < 0 || (!double.IsNaN(minWidth) && newWidth < minWidth))
            {
                return false;
            }

            var maxWidth = target.MaxWidth;
            if (!double.IsNaN(maxWidth) && newWidth > maxWidth)
            {
                return false;
            }

            if (newWidth <= parentActualWidth)
            {
                return false;
            }

            return true;
        }
    }
}