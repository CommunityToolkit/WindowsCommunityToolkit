using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.Foundation;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class Carousel
    {
        private double _offset;
        private double _slotWidth = 1;

        private void BuildSlots()
        {
            if (_container != null)
            {
                int count = MaxItems + 2;

                _container.Children.Clear();
                for (int n = 0; n < count; n++)
                {
                    var control = new CarouselSlot
                    {
                        ContentTemplate = ContentTemplate,
                        ItemClickCommand = ItemClickCommand,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Stretch,
                        UseLayoutRounding = false
                    };
                    _container.Children.Add(control);
                    control.MoveX(n);
                }
            }
        }

        private IEnumerable<Point> GetPositions(double slotWidth)
        {
            double x0 = GetLeftBound();
            for (int n = 0; n < (MaxItems + 2); n++)
            {
                yield return new Point(x0, 0);
                x0 += slotWidth;
            }
        }

        public void MoveNext()
        {
            SelectedIndex = SelectedIndex.IncMod(_items.Count);
        }
        public void MovePrev()
        {
            SelectedIndex = SelectedIndex.DecMod(_items.Count);
        }

        public void AnimateNext(double duration = 50)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(-delta, duration);
        }
        public void AnimatePrev(double duration = 50)
        {
            double delta = _slotWidth - _offset;
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(delta, duration);
        }

        public async void AnimateNextPage(double duration = 50)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            for (int n = 0; n < MaxItems * 4; n++)
            {
                MoveOffsetInternal(-delta / 4.0);
                await System.Threading.Tasks.Task.Delay(10);
            }
        }
        public async void AnimatePrevPage(double duration = 50)
        {
            double delta = _slotWidth - _offset;
            delta = delta < 1.0 ? _slotWidth : delta;
            for (int n = 0; n < MaxItems * 4; n++)
            {
                MoveOffsetInternal(delta / 4.0);
                await System.Threading.Tasks.Task.Delay(10);
            }
        }
    }
}
