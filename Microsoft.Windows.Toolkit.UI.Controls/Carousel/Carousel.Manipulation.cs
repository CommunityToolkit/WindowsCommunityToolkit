using System;
using System.Linq;

using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class Carousel
    {
        private void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            double velocity = e.Velocities.Linear.X;

            if (Math.Abs(velocity) > 2.0)
            {
                double offset = Math.Abs(_offset);
                if (Math.Sign(velocity) > 0)
                {
                    e.TranslationBehavior.DesiredDisplacement = _slotWidth * this.MaxItems - offset;
                }
                else
                {
                    e.TranslationBehavior.DesiredDisplacement = _slotWidth * (this.MaxItems - 1) + offset;
                }
            }
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            MoveOffset(e.Delta.Translation.X);
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_offset > 0.9 && _offset < _slotWidth - 0.9)
            {
                if (_offset < _slotWidth / 2.0)
                {
                    AnimateNext(150);
                }
                else
                {
                    AnimatePrev(150);
                }
            }
        }

        private DateTime _idleTime = DateTime.MinValue;

        private void MoveOffset(double delta, double duration = 0)
        {
            if (duration > 0)
            {
                if (DateTime.Now > _idleTime)
                {
                    _idleTime = DateTime.Now.AddMilliseconds(duration + 50);
                    MoveOffsetInternal(delta, duration);
                }
            }
            else
            {
                MoveOffsetInternal(delta);
            }
        }

        private void MoveOffsetInternal(double delta, double duration = 0)
        {
            if (_items.Count > 0)
            {
                double x0 = GetLeftBound();
                double x1 = Math.Round(x0 + _slotWidth * (MaxItems + 2), 2);

                int newIndex = this.SelectedIndex;
                var controls = _container.Children.Cast<CarouselSlot>().ToArray();
                for (int n = 0; n < controls.Length; n++)
                {
                    var control = controls[n];
                    var x = Math.Round(control.X + delta, 2);
                    if (x < x0 - 1)
                    {
                        double inc = x - x0;
                        control.MoveX(x1 + inc);
                        control.Content = _items[(this.SelectedIndex + (MaxItems + 1)).Mod(_items.Count)];
                        newIndex = this.SelectedIndex.IncMod(_items.Count);
                    }
                    else if (x > x1 - 1)
                    {
                        double inc = x - x1;
                        control.MoveX(x0 + inc);
                        control.Content = _items[(this.SelectedIndex - 2).Mod(_items.Count)];
                        newIndex = this.SelectedIndex.DecMod(_items.Count);
                    }
                    else
                    {
                        control.MoveX(x, duration);
                    }
                }
                _offset = Math.Round((_offset + delta).Mod(_slotWidth), 2);

                _disableSelectedIndexCallback = true;
                this.SelectedIndex = newIndex;
                _disableSelectedIndexCallback = false;
            }
        }

        private double GetLeftBound()
        {
            double contentWidth = _slotWidth * (MaxItems + 2);
            switch (this.AlignmentX)
            {
                case AlignmentX.Left:
                    return -Math.Round(_slotWidth, 2);
                case AlignmentX.Right:
                    contentWidth -= _slotWidth;
                    return Math.Round(_container.ActualWidth - contentWidth, 2);
                case AlignmentX.Center:
                default:
                    return -Math.Round((contentWidth - _container.ActualWidth) / 2.0, 2);
            }
        }
    }
}
