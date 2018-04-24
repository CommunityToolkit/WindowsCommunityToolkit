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

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenu is based on a SplitView control. By default it contains a HamburgerButton and a ListView to display menu items.
    /// </summary>
    public partial class HamburgerMenu
    {
        /// <summary>
        /// Identifies the <see cref="OpenPaneLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register(nameof(OpenPaneLength), typeof(double), typeof(HamburgerMenu), new PropertyMetadata(320.0));

        /// <summary>
        /// Identifies the <see cref="PanePlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PanePlacementProperty = DependencyProperty.Register(nameof(PanePlacement), typeof(SplitViewPanePlacement), typeof(HamburgerMenu), new PropertyMetadata(SplitViewPanePlacement.Left));

        /// <summary>
        /// Identifies the <see cref="DisplayMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register(nameof(DisplayMode), typeof(SplitViewDisplayMode), typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.CompactInline));

        /// <summary>
        /// Identifies the <see cref="CompactPaneLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register(nameof(CompactPaneLength), typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));

        /// <summary>
        /// Identifies the <see cref="PaneForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PaneForegroundProperty = DependencyProperty.Register(nameof(PaneForeground), typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PaneBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register(nameof(PaneBackground), typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="IsPaneOpen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(nameof(IsPaneOpen), typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null, OnItemsSourceChanged));

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ItemTemplateSelector"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null, OnSelectedItemChanged));

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(HamburgerMenu), new PropertyMetadata(-1, OnSelectedIndexChanged));

        /// <summary>
        /// Identifies the <see cref="UseNavigationViewWhenPossible"/> dependency property
        /// </summary>
        public static readonly DependencyProperty UseNavigationViewWhenPossibleProperty =
            DependencyProperty.Register("UseNavigationViewWhenPossible", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false, OnUseNavigationViewWhenPossibleChanged));

        /// <summary>
        /// Identifies the <see cref="UseNavigationViewSettingsWhenPossible"/> dependency property
        /// </summary>
        public static readonly DependencyProperty UseNavigationViewSettingsWhenPossibleProperty = DependencyProperty.Register("UseNavigationViewSettingsWhenPossible", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the width of the pane when it's fully expanded.
        /// </summary>
        public double OpenPaneLength
        {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies whether the pane is shown on the right or left side of the control.
        /// </summary>
        public SplitViewPanePlacement PanePlacement
        {
            get { return (SplitViewPanePlacement)GetValue(PanePlacementProperty); }
            set { SetValue(PanePlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies how the pane and content areas are shown.
        /// </summary>
        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the pane in its compact display mode.
        /// </summary>
        public double CompactPaneLength
        {
            get { return (double)GetValue(CompactPaneLengthProperty); }
            set { SetValue(CompactPaneLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Brush to apply to the foreground of the Pane area of the control
        /// (specifically, the hamburger button foreground).
        /// </summary>
        public Brush PaneForeground
        {
            get { return (Brush)GetValue(PaneForegroundProperty); }
            set { SetValue(PaneForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Brush to apply to the background of the Pane area of the control.
        /// </summary>
        public Brush PaneBackground
        {
            get { return (Brush)GetValue(PaneBackgroundProperty); }
            set { SetValue(PaneBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a value that specifies whether the pane is expanded to its full width.
        /// </summary>
        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object source used to generate the content of the menu.
        /// </summary>
        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each item.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplateSelector used to display each item.
        /// </summary>
        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets the collection used to generate the content of the items list.
        /// </summary>
        /// <exception cref="Exception">
        /// Exception thrown if ButtonsListView is not yet defined.
        /// </exception>
        public ItemCollection Items
        {
            get
            {
                if (_buttonsListView == null)
                {
                    throw new Exception("ButtonsListView is not defined yet. Please use ItemsSource instead.");
                }

                return _buttonsListView.Items;
            }
        }

        /// <summary>
        /// Gets or sets the selected menu item.
        /// </summary>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected menu index.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the HamburgerMenu should use the NavigationView when possible (Fall Creators Update and above)
        /// When set to true and the device supports NavigationView, the HamburgerMenu will use a template based on NavigationView
        /// </summary>
        public bool UseNavigationViewWhenPossible
        {
            get { return (bool)GetValue(UseNavigationViewWhenPossibleProperty); }
            set { SetValue(UseNavigationViewWhenPossibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the HamburgerMenu should try and automatically detect if any of the OptionsItems represent settings. If they do, the IsSettingsEnabled property of the NavigationView control will be set and the detected item invoked appropriately. (Fall Creators Update and above)
        /// If an item is not detected automatically, the detection can be triggered by adding a Tag property with the value "setting" to the appropriate OptionsItem.
        /// This property is ignored if UseNavigationViewWhenPossible is false.
        /// </summary>
        public bool UseNavigationViewSettingsWhenPossible
        {
            get { return (bool)GetValue(UseNavigationViewSettingsWhenPossibleProperty); }
            set { SetValue(UseNavigationViewSettingsWhenPossibleProperty, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HamburgerMenu hamburgerMenu && hamburgerMenu.UsingNavView)
            {
                hamburgerMenu.NavViewSetItemsSource();
            }
        }

        private static void OnUseNavigationViewWhenPossibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as HamburgerMenu;
            if (menu == null)
            {
                return;
            }

            if (menu.UseNavigationViewWhenPossible && HamburgerMenu.IsNavigationViewSupported)
            {
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new System.Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls/HamburgerMenu/HamburgerMenuNavViewTemplate.xaml");
                menu._previousTemplateUsed = menu.Template;
                menu.Template = dict["HamburgerMenuNavViewTemplate"] as ControlTemplate;
            }
            else if (!menu.UseNavigationViewWhenPossible &&
                     e.OldValue is bool oldValue &&
                     oldValue &&
                     menu._previousTemplateUsed != null)
            {
                menu.Template = menu._previousTemplateUsed;
            }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HamburgerMenu menu && menu.UsingNavView)
            {
                menu.NavViewSetSelectedItem(e.NewValue);
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HamburgerMenu menu && menu.UsingNavView)
            {
                if (menu.ItemsSource is IEnumerable<object> items)
                {
                    menu.NavViewSetSelectedItem((int)e.NewValue >= 0 ? items.ElementAt((int)e.NewValue) : null);
                }
            }
        }

        private static void OnSelectedOptionsIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HamburgerMenu menu && menu.UsingNavView)
            {
                if (menu.ItemsSource is IEnumerable<object> options)
                {
                    menu.NavViewSetSelectedItem((int)e.NewValue >= 0 ? options.ElementAt((int)e.NewValue) : null);
                }
            }
        }
    }
}
