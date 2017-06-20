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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A panel used by the <see cref="OrbitView"/> control
    /// </summary>
    public class OrbitViewPanel : Panel
    {
        private OrbitView _OrbitView;

        /// <summary>
        /// Event raised when a single element is arranged
        /// </summary>
        public event EventHandler<OrbitViewPanelItemArrangedArgs> ItemArranged;

        /// <summary>
        /// Event raised when all elements are arranged
        /// </summary>
        public event EventHandler<OrbitViewPanelItemsArrangedArgs> ItemsArranged;

        /// <summary>
        /// Gets the Current <see cref="OrbitView"/> control
        /// </summary>
        public OrbitView OrbitView
        {
            get
            {
                if (_OrbitView != null)
                {
                    return _OrbitView;
                }

                _OrbitView = this.FindVisualAscendant<OrbitView>();

                if (_OrbitView == null)
                {
                    throw new Exception("This OrbitViewPanel must be used as an ItemsPanel in a OrbitView control");
                }

                return _OrbitView;
            }
        }

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

        protected override Size ArrangeOverride(Size finalSize)
        {
            var angle = 2 * Math.PI / Children.Count;

            var minDistance = 100;
            var maxDistance = Math.Max(minDistance, (Math.Min(finalSize.Width, finalSize.Height) - OrbitView.MaxItemSize) / 2);

            var elementsProperties = new List<OrbitViewElementProperties>();

            for (var i = 0; i < Children.Count; i++)
            {
                var element = Children.ElementAt(i);

                OrbitViewItem OrbitViewItem = null;
                if (element is FrameworkElement)
                {
                    OrbitViewItem = (element as FrameworkElement).DataContext as OrbitViewItem;
                }

                var d = OrbitViewItem != null && OrbitViewItem.Distance >= 0 ? OrbitViewItem.Distance : 0.5;
                d = Math.Min(d, 1d);

                var distance = (d * (maxDistance - minDistance)) + minDistance;
                var x = distance * Math.Cos((angle * i) + (angle / 2));
                var y = distance * Math.Sin((angle * i) + (angle / 2));

                var x_normalized = (finalSize.Width / 2) + x - (element.DesiredSize.Width / 2);
                var y_normalized = (finalSize.Height / 2) - y - (element.DesiredSize.Height / 2);
                var point = new Point(x_normalized, y_normalized);

                element.Arrange(new Rect(point, element.DesiredSize));

                var elementProperties = new OrbitViewElementProperties()
                {
                    XYFromCenter = new Point(x, y),
                    DistanceFromCenter = distance,
                    Element = element
                };
                elementsProperties.Add(elementProperties);

                if (ItemArranged != null)
                {
                    var args = new OrbitViewPanelItemArrangedArgs()
                    {
                        ElementProperties = elementProperties,
                        ItemIndex = i
                    };
                    ItemArranged.Invoke(this, args);
                }
            }

            ItemsArranged?.Invoke(this, new OrbitViewPanelItemsArrangedArgs() { Elements = elementsProperties });

            return finalSize;
        }
    }
}
