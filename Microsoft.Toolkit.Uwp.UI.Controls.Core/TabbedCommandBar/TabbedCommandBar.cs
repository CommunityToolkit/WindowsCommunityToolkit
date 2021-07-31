// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewSelectionChangedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A basic TabbedCommandBar control that houses <see cref="TabbedCommandBarItem"/>s
    /// </summary>
    [ContentProperty(Name = nameof(MenuItems))]
    [TemplatePart(Name = "PART_TabbedCommandBarContent", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_TabbedCommandBarContentBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_TabChangedStoryboard", Type = typeof(Storyboard))]
    public class TabbedCommandBar : NavigationView
    {
        private ContentControl _tabbedCommandBarContent = null;
        private Border _tabbedCommandBarContentBorder = null;
        private Storyboard _tabChangedStoryboard = null;

        /// <summary>
        /// The last selected <see cref="TabbedCommandBarItem"/>.
        /// </summary>
        private TabbedCommandBarItem _previousSelectedItem = null;
        private long _visibilityChangedToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedCommandBar"/> class.
        /// </summary>
        public TabbedCommandBar()
        {
            DefaultStyleKey = typeof(TabbedCommandBar);
            DefaultStyleResourceUri = new System.Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls.Core/Themes/Generic.xaml");

            SelectionChanged += SelectedItemChanged;
            Loaded += TabbedCommandBar_Loaded;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_tabbedCommandBarContent != null)
            {
                _tabbedCommandBarContent.Content = null;
            }

            // Get TabbedCommandBarContent first, since setting SelectedItem requires it
            _tabbedCommandBarContent = GetTemplateChild("PART_TabbedCommandBarContent") as ContentControl;
            _tabbedCommandBarContentBorder = GetTemplateChild("PART_TabbedCommandBarContentBorder") as Border;
            _tabChangedStoryboard = GetTemplateChild("TabChangedStoryboard") as Storyboard;

            // TODO: We could maybe optimize and use a lower-level Loaded event for what's hosting the MenuItems
            // to set SelectedItem, but then we may have to pull in another template part, so think we're OK
            // to do the Loaded event at the top level.
        }

        private void TabbedCommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            // We need to select the item after the template is realized, otherwise the SelectedItem's
            // DataTemplate bindings don't properly navigate the visual tree.
            SelectedItem = MenuItems.FirstOrDefault();
        }

        private void SelectedItemChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = sender.SelectedItem as TabbedCommandBarItem;
            if (item == null || item.Visibility == Visibility.Collapsed)
            {
                // If the item is now hidden, select the first item instead.
                // I can't think of any way that the visibiltiy would be null
                // and still be selectable, but let's handle it just in case.
                sender.SelectedItem = sender.MenuItems.FirstOrDefault();
                return;
            }

            // Remove the visibility PropertyChanged handler from the
            // previously selected item
            if (_previousSelectedItem != null)
            {
                _previousSelectedItem.UnregisterPropertyChangedCallback(TabbedCommandBarItem.VisibilityProperty, _visibilityChangedToken);
            }

            // Register a new visibility PropertyChangedcallback for the
            // currently selected item
            _previousSelectedItem = item;
            _visibilityChangedToken =
            _previousSelectedItem.RegisterPropertyChangedCallback(TabbedCommandBarItem.VisibilityProperty, SelectedItemVisibilityChanged);

            // Set the TabbedCommandBar background and start the transition animation
            _tabChangedStoryboard?.Begin();
        }

        private void SelectedItemVisibilityChanged(DependencyObject sender, DependencyProperty dp)
        {
            // If the item is not visible, default to the first tab
            if (sender.GetValue(dp) is Visibility vis && vis == Visibility.Collapsed)
            {
                // FIXME: This will cause WinUI to throw an exception if run
                // when the tabs overflow
                SelectedItem = MenuItems.FirstOrDefault();
            }
        }
    }
}