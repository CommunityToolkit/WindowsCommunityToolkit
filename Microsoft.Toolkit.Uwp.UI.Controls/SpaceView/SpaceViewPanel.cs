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
    internal class SpaceViewPanel : Panel
    {
        private SpaceView _spaceView;

        public event EventHandler<SpaceViewPanelItemArrangedArgs> ItemArranged;

        /// <summary>
        /// Gets the Current SpaceView control
        /// </summary>
        public SpaceView SpaceView
        {
            get
            {
                if (_spaceView != null)
                {
                    return _spaceView;
                }

                _spaceView = this.FindVisualAscendant<SpaceView>();

                if (_spaceView == null)
                {
                    throw new Exception("This SpaceViewPanel must be used as an ItemsPanel in a SpaceView control");
                }

                return _spaceView;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var child in Children)
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var angle = 2 * Math.PI / Children.Count;

            var minDistance = 100;
            var maxDistance = Math.Max(minDistance, (Math.Min(finalSize.Width, finalSize.Height) - SpaceView.MaxItemSize) / 2);

            for (var i = 0; i < Children.Count; i++)
            {
                var element = Children.ElementAt(i);

                SpaceViewItem spaceViewItem = null;
                if (element is FrameworkElement)
                {
                    spaceViewItem = (element as FrameworkElement).DataContext as SpaceViewItem;
                }

                var d = spaceViewItem != null && spaceViewItem.Distance >= 0 ? spaceViewItem.Distance : 0.5;
                d = Math.Min(d, 1d);

                var distance = (d * (maxDistance - minDistance)) + minDistance;
                var x = distance * Math.Cos((angle * i) + (angle / 2));
                var y = distance * Math.Sin((angle * i) + (angle / 2));

                var x_normalized = (finalSize.Width / 2) + x - (element.DesiredSize.Width / 2);
                var y_normalized = (finalSize.Height / 2) + y - (element.DesiredSize.Height / 2);
                var point = new Point(x_normalized, y_normalized);

                element.Arrange(new Rect(point, element.DesiredSize));

                if (ItemArranged != null)
                {
                    var args = new SpaceViewPanelItemArrangedArgs()
                    {
                        XYFromCenter = new Point(x, y),
                        DistanceFromCenter = distance,
                        Element = element,
                        ItemIndex = i
                    };
                    ItemArranged.Invoke(this, args);
                }
            }

            return finalSize;
        }
    }
}
