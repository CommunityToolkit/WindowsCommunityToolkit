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
using System.Collections.Generic;

using Windows.Foundation;
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class Carousel
    {
        private double _offset;
        private double _slotWidth = 1;

        private void BuildSlots()
        {
            if (_container == null)
            {
                return;
            }

            int count = MaxItemsPlusOffset;

            _container.Children.Clear();
            for (int n = 0; n < count; n++)
            {
                var control = new CarouselSlot
                {
                    ContentTemplate = ContentTemplate, 
                    ItemClickCommand = ItemClickCommand, 
                    HorizontalContentAlignment = HorizontalAlignment.Stretch, 
                    VerticalContentAlignment = VerticalAlignment.Stretch, 
                    UseLayoutRounding = true
                };
                _container.Children.Add(control);
                control.MoveX(n);
            }
        }

        private IEnumerable<Point> GetPositions(double slotWidth)
        {
            double x0 = GetLeftBound();
            for (int n = 0; n < MaxItemsPlusOffset; n++)
            {
                yield return new Point(x0, 0);
                x0 += slotWidth;
            }
        }

        /// <summary>
        /// Select the next item in the list.
        /// </summary>
        public void MoveNext()
        {
            SelectedIndex = SelectedIndex.IncMod(_items.Count);
        }

        /// <summary>
        /// Select the previous item in the list.
        /// </summary>
        public void MovePrev()
        {
            SelectedIndex = SelectedIndex.DecMod(_items.Count);
        }

        /// <summary>
        /// Move to next item with an animation.
        /// </summary>
        /// <param name="duration">Animation's duration.</param>
        public void AnimateNext(double duration = 50)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(-delta, duration);
        }

        /// <summary>
        /// Move to the previous item with an animation.
        /// </summary>
        /// <param name="duration">Animation's duration.</param>
        public void AnimatePrev(double duration = 50)
        {
            double delta = _slotWidth - _offset;
            delta = delta < 1.0 ? _slotWidth : delta;
            MoveOffset(delta, duration);
        }

        /// <summary>
        /// Move to the next page with an animation.
        /// </summary>
        /// <param name="duration">Animation's duration.</param>
        public void AnimateNextPage(double duration = 50)
        {
            double delta = Math.Abs(_offset);
            delta = delta < 1.0 ? _slotWidth : delta;
            for (int n = 0; n < MaxItems * 4; n++)
            {
                MoveOffsetInternal(-delta / 4.0, duration);
            }
        }

        /// <summary>
        /// Move to the previous page with an animation.
        /// </summary>
        /// <param name="duration">Animation's duration.</param>
        public void AnimatePrevPage(double duration = 50)
        {
            double delta = _slotWidth - _offset;
            delta = delta < 1.0 ? _slotWidth : delta;
            for (int n = 0; n < MaxItems * 4; n++)
            {
                MoveOffsetInternal(delta / 4.0, duration);
            }
        }
    }
}
