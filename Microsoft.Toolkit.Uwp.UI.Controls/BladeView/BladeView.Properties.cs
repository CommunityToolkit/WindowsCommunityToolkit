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

using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="BladeItem"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeView
    {
        /// <summary>
        /// Identifies the <see cref="ActiveBlades"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActiveBladesProperty = DependencyProperty.Register(nameof(ActiveBlades), typeof(IList<BladeItem>), typeof(BladeView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="FullScreenBlades"/> attached property.
        /// </summary>
        public static readonly DependencyProperty FullScreenBladesProperty = DependencyProperty.RegisterAttached(nameof(FullScreenBlades), typeof(bool), typeof(BladeView), new PropertyMetadata(false, FullScreenBladesChangedCallback));

        /// <summary>
        /// Gets or sets a collection of visible blades
        /// </summary>
        public IList<BladeItem> ActiveBlades
        {
            get { return (IList<BladeItem>)GetValue(ActiveBladesProperty); }
            set { SetValue(ActiveBladesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether blades are full screen
        /// </summary>
        public bool FullScreenBlades
        {
            get { return (bool)GetValue(FullScreenBladesProperty); }
            set { SetValue(FullScreenBladesProperty, value); }
        }

        private static void FullScreenBladesChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            // Check if value changed
            if (e.NewValue != e.OldValue)
            {
                var bladeView = (BladeView)dependencyObject;
                var bladeScrollViewer = bladeView.GetScrollViewer();

                if (bladeView.FullScreenBlades)
                {
                    // Cache previous values of blade items properties (width & height)
                    bladeView._cachedBladeItemSizes.Clear();
                    foreach (BladeItem bladeItem in bladeView.Items)
                    {
                        bladeView._cachedBladeItemSizes.Add(bladeItem, new Vector2((float)bladeItem.Width, (float)bladeItem.Height));
                    }

                    // Change ScrollView behavior
                    bladeScrollViewer.HorizontalSnapPointsType = SnapPointsType.MandatorySingle;
                }
                else
                {
                    // Reset blade items properties & clear cache
                    foreach (var kvBladeItemSize in bladeView._cachedBladeItemSizes)
                    {
                        kvBladeItemSize.Key.Width = kvBladeItemSize.Value.X;
                        kvBladeItemSize.Key.Height = kvBladeItemSize.Value.Y;
                    }

                    bladeView._cachedBladeItemSizes.Clear();

                    // Change ScrollView behavior
                    bladeScrollViewer.HorizontalSnapPointsType = SnapPointsType.Optional;
                }

                // Execute change of blade item size
                bladeView.AdjustBladeItemSize();
            }
        }
    }
}
