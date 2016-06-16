// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************
using System;
using System.Linq;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class Carousel
    {
        const int OffsetScale = 2;
        private int MaxItemsPlusOffset => MaxItems + 2 * OffsetScale;

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
                double x1 = Math.Round(x0 + _slotWidth * MaxItemsPlusOffset, 2);

                int newIndex = SelectedIndex;
                var controls = _container.Children.Cast<CarouselSlot>().ToArray();
                foreach (var control in controls)
                {
                    var x = Math.Round(control.X + delta, 2);
                    if (x < x0 - 1)
                    {
                        double inc = x - x0;
                        control.MoveX(x1 + inc);
                        control.Content = _items[(SelectedIndex + MaxItemsPlusOffset - 1).Mod(_items.Count)];
                        newIndex = SelectedIndex.IncMod(_items.Count);
                    }
                    else if (x > x1 - 1)
                    {
                        double inc = x - x1;
                        control.MoveX(x0 + inc);
                        control.Content = _items[(SelectedIndex - 2).Mod(_items.Count)];
                        newIndex = SelectedIndex.DecMod(_items.Count);
                    }
                    else
                    {
                        control.MoveX(x, duration);
                    }
                }

                _offset = Math.Round((_offset + delta).Mod(_slotWidth), 2);

                _disableSelectedIndexCallback = true;
                SelectedIndex = newIndex;
                _disableSelectedIndexCallback = false;
            }
        }

        private double GetLeftBound()
        {
            double contentWidth = _slotWidth * MaxItemsPlusOffset;
            switch (AlignmentX)
            {
                case AlignmentX.Left:
                    return -Math.Round(_slotWidth, 2) * OffsetScale;
                case AlignmentX.Right:
                    contentWidth -= _slotWidth * OffsetScale;
                    return Math.Round(_container.ActualWidth - contentWidth, 2);
                default:
                    return -Math.Round((contentWidth - _container.ActualWidth) / 2.0, 2);
            }
        }
    }
}
