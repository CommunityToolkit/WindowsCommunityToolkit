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
        // Name of Content area in NavigationView Template.
        private const string CONTENT_GRID = "ContentGrid";

        /// <summary>
        /// Gets the index of the selected <see cref="NavigationViewItem"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.NavigationView"/>.</param>
        /// <returns>The selected index.</returns>
        public static int GetSelectedIndex(NavigationView obj)
        {
            return (int)obj.GetValue(SelectedIndexProperty);
        }

        /// <summary>
        /// Sets the index of the selected <see cref="NavigationViewItem"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.NavigationView"/>.</param>
        /// <param name="value">The index to select.</param>
        public static void SetSelectedIndex(NavigationView obj, int value)
        {
            obj.SetValue(SelectedIndexProperty, value);
        }

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding the selected index of a <see cref="NavigationView"/>.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.RegisterAttached("SelectedIndex", typeof(int), typeof(NavigationViewExtensions), new PropertyMetadata(-1, OnSelectedIndexChanged));

        /// <summary>
        /// Gets the behavior to collapse the content when clicking the already selected <see cref="NavigationViewItem"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.NavigationView"/>.</param>
        /// <returns>True if the feature is on.</returns>
        public static bool GetCollapseOnClick(NavigationView obj)
        {
            return (bool)obj.GetValue(CollapseOnClickProperty);
        }

        /// <summary>
        /// Sets the behavior to collapse the content when clicking the already selected <see cref="NavigationViewItem"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.NavigationView"/>.</param>
        /// <param name="value">True to turn on this feature.</param>
        public static void SetCollapseOnClick(NavigationView obj, bool value)
        {
            obj.SetValue(CollapseOnClickProperty, value);
        }

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for enabling the behavior to collapse the <see cref="NavigationView"/> content when the same selected item is invoked again (click or tap).
        /// </summary>
        public static readonly DependencyProperty CollapseOnClickProperty =
            DependencyProperty.RegisterAttached("CollapseOnClick", typeof(bool), typeof(NavigationViewExtensions), new PropertyMetadata(false, OnCollapseOnClickChanged));

        private static void OnCollapseOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // This should always be a NavigationView.
            var navview = (NavigationView)d;

            navview.ItemInvoked -= Navview_ItemInvoked;

            if ((bool?)e.NewValue == true)
            {
                // Listen for clicks on navigation items
                navview.ItemInvoked += Navview_ItemInvoked;
            }
            else
            {
                // Make sure we're visible if we toggle this off.
                var content = navview.FindDescendantByName(CONTENT_GRID);

                if (content != null)
                {
                    content.Visibility = Visibility.Visible;
                }
            }
        }

        private static void Navview_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var content = sender.FindDescendantByName(CONTENT_GRID);

            if (content != null)
            {
                // If we click the item we already have selected, we want to collapse our content
                if (sender.SelectedItem != null && args.InvokedItem.Equals(((NavigationViewItem)sender.SelectedItem).Content))
                {
                    // We need to dispatch this so the underlying selection event from our invoke processes.
                    // Otherwise, we just end up back where we started.  We don't care about waiting for this to finish.
                    #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    sender.Dispatcher.RunIdleAsync((e) =>
                    {
                         sender.SelectedItem = null;
                    });
                    #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                    content.Visibility = Visibility.Collapsed;
                }
                else
                {
                    content.Visibility = Visibility.Visible;
                }
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navview = (NavigationView)d;

            navview.Loaded -= Navview_Loaded;
            Navview_Loaded(d, null); // For changes
            navview.Loaded += Navview_Loaded;

            navview.SelectionChanged -= Obj_SelectionChanged;
            navview.SelectionChanged += Obj_SelectionChanged;
        }

        private static void Navview_Loaded(object sender, RoutedEventArgs e)
        {
            var navview = (NavigationView)sender;

            int value = GetSelectedIndex(navview);

            if (value >= 0 && value < navview.MenuItems.Count)
            {
                // Only update if we need to.
                if (navview.SelectedItem == null || !navview.SelectedItem.Equals(navview.MenuItems[value] as NavigationViewItem))
                {
                    navview.SelectedItem = navview.MenuItems[value];
                }
            }
            else
            {
                navview.SelectedItem = null;
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
