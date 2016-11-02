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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
        /// Identifies the <see cref="ToggleBlade"/> attached property.
        /// </summary>
        [Deprecated("This property has been deprecated. Please use the IsOpen property of the BladeItem.", DeprecationType.Deprecate, 1)]
        public static readonly DependencyProperty ToggleBladeProperty = DependencyProperty.RegisterAttached(nameof(ToggleBlade), typeof(string), typeof(BladeControl), new PropertyMetadata(null));

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

        /// <summary>
        /// Sets the ID of a blade to toggle on a UIElement tap
        /// </summary>
        /// <param name="element">The UIElement to toggle the blade</param>
        /// <param name="value">The ID of the blade we want to toggle</param>
        [Deprecated("This property has been deprecated. Please use the IsOpen property of the BladeItem.", DeprecationType.Deprecate, 1)]
        public static void SetToggleBlade(UIElement element, string value)
        {
            element.Tapped -= ToggleBlade;
            element.Tapped += ToggleBlade;

            element.SetValue(ToggleBladeProperty, value);
        }

        /// <summary>
        /// Gets the ID of a blade to toggle on a UIElement tap
        /// </summary>
        /// <param name="element">The UIElement to toggle the blade</param>
        /// <returns>The ID of the blade</returns>
        [Deprecated("This property has been deprecated. Please use the IsOpen property of the BladeItem.", DeprecationType.Deprecate, 1)]
        public static string GetToggleBlade(UIElement element)
        {
            return element.GetValue(ToggleBladeProperty).ToString();
        }

        /// <summary>
        /// Fired when the deprecated Blades property changes.
        /// Handles moving items from the Blades collection to the Items collection.
        /// Subscribes to the CollectionChanged event if Blades implements INotifyCollectionChanged
        /// in order to add or remove Blades from the Items collection.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The event args.</param>
        private static void OnBladesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bladeControl = (BladeControl)d;
#pragma warning disable CS0618 // Type or member is obsolete
            IList<Blade> blades = bladeControl.Blades;
#pragma warning restore CS0618 // Type or member is obsolete
            if (blades != null)
            {
                foreach (var blade in blades)
                {
                    bladeControl.Items.Add(blade);
                }

                var collection = blades as INotifyCollectionChanged;
                if (collection != null)
                {
                    collection.CollectionChanged += bladeControl.OnBladeCollectionChanged;
                }
            }
        }

        private static void ToggleBlade(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            Button pressedButton = sender as Button;
#pragma warning disable CS0618 // Type or member is obsolete
            string bladeName = GetToggleBlade(pressedButton);
            BladeControl container = pressedButton.FindVisualAscendant<BladeControl>();
            var blade = container.Items.OfType<Blade>().FirstOrDefault(_ => _.BladeId == bladeName);
#pragma warning restore CS0618 // Type or member is obsolete

            if (blade == null)
            {
                throw new KeyNotFoundException($"Could not find a blade with ID {bladeName}");
            }

            blade.IsOpen = !blade.IsOpen;
        }

        private void OnBladeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete

        }
    }
}
