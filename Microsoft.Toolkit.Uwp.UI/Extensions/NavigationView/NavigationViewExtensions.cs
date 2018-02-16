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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Set of extensions for the <see cref="NavigationView"/> control.
    /// </summary>
    [Bindable]
    public class NavigationViewExtensions
    {
        /// <summary>
        /// Gets the index of the selected <see cref="NavigationViewItem"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.NavigationView"/>.</param>
        /// <returns>The selected index.</returns>
        public static int GetSelectedIndex(Windows.UI.Xaml.Controls.NavigationView obj)
        {
            return (int)obj.GetValue(SelectedIndexProperty);
        }

        /// <summary>
        /// Sets the index of the selected <see cref="NavigationViewItem"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.NavigationView"/>.</param>
        /// <param name="value">The index to select.</param>
        public static void SetSelectedIndex(Windows.UI.Xaml.Controls.NavigationView obj, int value)
        {
            obj.SetValue(SelectedIndexProperty, value);
        }

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding the selected index of a <see cref="NavigationView"/>.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.RegisterAttached("SelectedIndex", typeof(int), typeof(NavigationViewExtensions), new PropertyMetadata(-1, OnSelectedIndexChanged));

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navview = d as Windows.UI.Xaml.Controls.NavigationView;

            if (navview == null)
            {
                return;
            }

            navview.Loaded -= Navview_Loaded;
            Navview_Loaded(d, null); // For changes
            navview.Loaded += Navview_Loaded;

            navview.SelectionChanged -= Obj_SelectionChanged;
            navview.SelectionChanged += Obj_SelectionChanged;
        }

        private static void Navview_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is NavigationView navview)
            {
                int value = GetSelectedIndex(navview);

                if (value >= 0 && value < navview.MenuItems.Count && navview.SelectedItem != navview.MenuItems[value])
                {
                    navview.SelectedItem = navview.MenuItems[value];
                }
                else
                {
                    navview.SelectedItem = null;
                }
            }
        }

        private static void Obj_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (!args.IsSettingsSelected && args.SelectedItem != null)
            {
                var index = sender.MenuItems.IndexOf(args.SelectedItem);
                if (index != GetSelectedIndex(sender))
                {
                    SetSelectedIndex(sender, index);
                }
            }
            else
            {
                SetSelectedIndex(sender, -1);
            }
        }
    }
}
