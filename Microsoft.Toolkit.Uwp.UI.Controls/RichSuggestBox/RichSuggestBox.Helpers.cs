// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RichSuggestBox control extends <see cref="RichEditBox"/> control that suggests and embeds custom data in a rich document.
    /// </summary>
    public partial class RichSuggestBox
    {
        private static bool IsElementOnScreen(UIElement element, double offsetX = 0, double offsetY = 0)
        {
            var toWindow = element.TransformToVisual(Window.Current.Content);
            var windowBounds = ApplicationView.GetForCurrentView().VisibleBounds;
            var elementBounds = new Rect(offsetX, offsetY, element.ActualSize.X, element.ActualSize.Y);
            elementBounds = toWindow.TransformBounds(elementBounds);
            elementBounds.X += windowBounds.X;
            elementBounds.Y += windowBounds.Y;
            var displayInfo = DisplayInformation.GetForCurrentView();
            var scaleFactor = displayInfo.RawPixelsPerViewPixel;
            var displayHeight = displayInfo.ScreenHeightInRawPixels;
            return elementBounds.Top * scaleFactor >= 0 && elementBounds.Bottom * scaleFactor <= displayHeight;
        }

        private static bool IsElementInsideWindow(UIElement element, double offsetX = 0, double offsetY = 0)
        {
            var toWindow = element.TransformToVisual(Window.Current.Content);
            var windowBounds = ApplicationView.GetForCurrentView().VisibleBounds;
            windowBounds = new Rect(0, 0, windowBounds.Width, windowBounds.Height);
            var elementBounds = new Rect(offsetX, offsetY, element.ActualSize.X, element.ActualSize.Y);
            elementBounds = toWindow.TransformBounds(elementBounds);
            elementBounds.Intersect(windowBounds);
            return elementBounds.Height >= element.ActualSize.Y;
        }
    }
}
