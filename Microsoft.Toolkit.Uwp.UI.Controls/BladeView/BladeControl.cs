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
using System.Collections.ObjectModel;
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
    [Deprecated("The BladeControl class has been replaced with the BladeView class. Please use that going forward", DeprecationType.Deprecate, 1)]
    public class BladeControl : BladeView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BladeControl"/> class.
        /// </summary>
        public BladeControl()
        {
            Blades = new ObservableCollection<Blade>();
        }

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
        /// Gets or sets a collection of blades
        /// </summary>
        [Deprecated("This property has been replaced with the Items property of the control. It is no longer required to place content within the Blades property.", DeprecationType.Deprecate, 1)]
        public IList<Blade> Blades
        {
            get { return (IList<Blade>)GetValue(BladesProperty); }
            set { SetValue(BladesProperty, value); }
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
            IList<Blade> blades = bladeControl.Blades;
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
            string bladeName = GetToggleBlade(pressedButton);
            BladeControl container = pressedButton.FindAscendant<BladeControl>();
            var blade = container.Items.OfType<Blade>().FirstOrDefault(_ => _.BladeId == bladeName);

            if (blade == null)
            {
                throw new KeyNotFoundException($"Could not find a blade with ID {bladeName}");
            }

            blade.IsOpen = !blade.IsOpen;
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
