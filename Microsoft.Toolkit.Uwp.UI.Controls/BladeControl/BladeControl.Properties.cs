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
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="BladeItem"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeControl
    {
        /// <summary>
        /// Identifies the <see cref="Blades"/> dependency property.
        /// </summary>
        [Deprecated("This property has been replaced with the Items property of the control. It is no longer required to place content within the Blades property.", DeprecationType.Deprecate, 1)]
        public static readonly DependencyProperty BladesProperty = DependencyProperty.Register(nameof(Blades), typeof(IList<Blade>), typeof(BladeControl), new PropertyMetadata(null, OnBladesChanged));

        /// <summary>
        /// Identifies the <see cref="ActiveBlades"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActiveBladesProperty = DependencyProperty.Register(nameof(ActiveBlades), typeof(IList<BladeItem>), typeof(BladeControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a collection of visible blades
        /// </summary>
        public IList<BladeItem> ActiveBlades
        {
            get { return (IList<BladeItem>)GetValue(ActiveBladesProperty); }
            set { SetValue(ActiveBladesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of blades
        /// </summary>
        [Deprecated("This property has been replaced with the Items property of the control. It is no longer required to place content within the Blades property.", DeprecationType.Deprecate, 1)]
        public IList<Blade> Blades
        {
            get { return (IList<Blade>)GetValue(BladesProperty); }
            set { SetValue(BladesProperty, value); }
        }

        private static void OnBladesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bladeControl = (BladeControl)d;
            if (bladeControl.Blades != null)
            {
                foreach (var blade in bladeControl.Blades)
                {
                    bladeControl.Items.Add(blade);
                }

                var collection = bladeControl.Blades as INotifyCollectionChanged;
                if (collection != null)
                {
                    collection.CollectionChanged += bladeControl.OnBladeCollectionChanged;
                }
            }
        }

        private void OnBladeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var blade in e.OldItems.OfType<Blade>())
                {
                    Items.Remove(blade);
                }
            }

            if (e.NewItems != null)
            {
                foreach (var blade in e.NewItems.OfType<Blade>())
                {
                    Items.Add(blade);
                }
            }
        }
    }
}
