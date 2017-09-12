// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines an area where you can arrange child elements either horizontally or vertically, relative to each other.
    /// </summary>
    public partial class DockPanel : Panel
    {
        private static void DockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var dockPanel = senderElement?.FindParent<DockPanel>();

            dockPanel?.InvalidateArrange();
        }

        private static void LastChildFillChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dockPanel = (DockPanel)sender;
            dockPanel.InvalidateArrange();
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
            {
                return finalSize;
            }

            var currentBounds = new Rect(0, 0, finalSize.Width, finalSize.Height);
            var childrenCount = LastChildFill ? Children.Count - 1 : Children.Count;

            for (var index = 0; index < childrenCount; index++)
            {
                var child = Children[index];
                child.SetValue(Canvas.ZIndexProperty, Children.Count - index);
                var dock = (Dock)child.GetValue(DockProperty);
                double width, height;
                switch (dock)
                {
                    case Dock.Left:

                        width = child.DesiredSize.Width;
                        child.Arrange(new Rect(currentBounds.X, currentBounds.Y, width, currentBounds.Height - currentBounds.Y));
                        currentBounds.X += width;

                        break;
                    case Dock.Top:

                        height = child.DesiredSize.Height;
                        child.Arrange(new Rect(currentBounds.X, currentBounds.Y, currentBounds.Width - currentBounds.X, height));
                        currentBounds.Y += height;

                        break;
                    case Dock.Right:

                        width = child.DesiredSize.Width;
                        child.Arrange(new Rect(currentBounds.Width - width, currentBounds.Y, width, currentBounds.Height - currentBounds.Y));
                        currentBounds.Width -= (currentBounds.Width - width) > 0 ? width : 0;

                        break;
                    case Dock.Bottom:

                        height = child.DesiredSize.Height;
                        child.Arrange(new Rect(currentBounds.X, currentBounds.Height - height, currentBounds.Width - currentBounds.X, height));
                        currentBounds.Height -= (currentBounds.Height - height) > 0 ? height : 0;

                        break;
                }
            }

            if (LastChildFill)
            {
                var width = currentBounds.Width - currentBounds.X;
                var height = currentBounds.Height - currentBounds.Y;
                var child = Children[Children.Count - 1];
                child.Arrange(
                    new Rect(currentBounds.X, currentBounds.Y, width > 0 ? width : 0, height > 0 ? height : 0));
                child.SetValue(Canvas.ZIndexProperty, 0);
            }

            return finalSize;
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size availableSize)
        {
            var width = availableSize.Width;
            var height = availableSize.Height;

            if (double.IsInfinity(width))
            {
                width = Window.Current.Bounds.Width;
            }

            if (double.IsInfinity(height))
            {
                height = Window.Current.Bounds.Height;
            }

            var finalSize = new Size(width, height);

            foreach (var child in Children)
            {
                child.Measure(finalSize);
            }

            return finalSize;
        }
    }
}
