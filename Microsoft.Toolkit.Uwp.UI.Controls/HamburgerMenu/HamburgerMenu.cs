// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenu is based on a SplitView control. By default it contains a HamburgerButton and a ListView to display menu items.
    /// </summary>
    [TemplatePart(Name = "HamburgerButton", Type = typeof(Button))]
    [TemplatePart(Name = "ButtonsListView", Type = typeof(Windows.UI.Xaml.Controls.ListViewBase))]
    [TemplatePart(Name = "OptionsListView", Type = typeof(Windows.UI.Xaml.Controls.ListViewBase))]
    [Obsolete("The HamburgerMenu will be removed in a future major release. Please use the NavigationView control available in the Fall Creators Update")]
    public partial class HamburgerMenu : ContentControl
    {
        private Button _hamburgerButton;
        private Windows.UI.Xaml.Controls.ListViewBase _buttonsListView;
        private Windows.UI.Xaml.Controls.ListViewBase _optionsListView;

        private ControlTemplate _previousTemplateUsed;
        private object _navigationView;
        private object _settingsObject;

        private bool UsingNavView => UseNavigationViewWhenPossible && IsNavigationViewSupported;

        /// <summary>
        /// Gets a value indicating whether <see cref="NavigationView"/> is supported
        /// </summary>
        public static bool IsNavigationViewSupported { get; } = ApiInformation.IsTypePresent("Windows.UI.Xaml.Controls.NavigationView");

        /// <summary>
        /// Initializes a new instance of the <see cref="HamburgerMenu"/> class.
        /// </summary>
        public HamburgerMenu()
        {
            DefaultStyleKey = typeof(HamburgerMenu);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture children controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (PaneForeground == null)
            {
                PaneForeground = Foreground;
            }

            if (_hamburgerButton != null)
            {
                _hamburgerButton.Click -= HamburgerButton_Click;
            }

            if (_buttonsListView != null)
            {
                _buttonsListView.ItemClick -= ButtonsListView_ItemClick;
            }

            if (_optionsListView != null)
            {
                _optionsListView.ItemClick -= OptionsListView_ItemClick;
            }

            if (UsingNavView)
            {
                OnApplyTemplateNavView();
            }

            _hamburgerButton = (Button)GetTemplateChild("HamburgerButton");
            _buttonsListView = (Windows.UI.Xaml.Controls.ListViewBase)GetTemplateChild("ButtonsListView");
            _optionsListView = (Windows.UI.Xaml.Controls.ListViewBase)GetTemplateChild("OptionsListView");

            if (_hamburgerButton != null)
            {
                _hamburgerButton.Click += HamburgerButton_Click;
            }

            if (_buttonsListView != null)
            {
                _buttonsListView.ItemClick += ButtonsListView_ItemClick;
            }

            if (_optionsListView != null)
            {
                _optionsListView.ItemClick += OptionsListView_ItemClick;
            }

            base.OnApplyTemplate();
        }

        private void OnApplyTemplateNavView()
        {
            if (_navigationView is NavigationView navView)
            {
                navView.ItemInvoked -= NavigationViewItemInvoked;
                navView.SelectionChanged -= NavigationViewSelectionChanged;
                navView.Loaded -= NavigationViewLoaded;
            }

            navView = GetTemplateChild("NavView") as NavigationView;

            if (navView != null)
            {
                navView.ItemInvoked += NavigationViewItemInvoked;
                navView.SelectionChanged += NavigationViewSelectionChanged;
                navView.Loaded += NavigationViewLoaded;
                navView.MenuItemTemplateSelector = new HamburgerMenuNavViewItemTemplateSelector(this);
                _navigationView = navView;

                OnItemsSourceChanged(this, null);

                if (SelectedItem != null)
                {
                    NavViewSetSelectedItem(SelectedItem);
                }
                else if (SelectedOptionsItem != null)
                {
                    NavViewSetSelectedItem(SelectedOptionsItem);
                }
            }
        }

        private void NavViewSetItemsSource()
        {
            if (UsingNavView && _navigationView is NavigationView navView && navView != null)
            {
                var items = ItemsSource as IEnumerable<object>;
                var options = OptionsItemsSource as IEnumerable<object>;

                List<object> combined = new List<object>();

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        combined.Add(item);
                    }
                }

                if (options != null)
                {
                    if (options.Count() > 0)
                    {
                        combined.Add(new NavigationViewItemSeparator());
                    }

                    foreach (var option in options)
                    {
                        if (!UseNavigationViewSettingsWhenPossible)
                        {
                            combined.Add(option);
                        }
                        else
                        {
                            if (_settingsObject == null && IsSettingsItem(option))
                            {
                                _settingsObject = option;
                                navView.IsSettingsVisible = true;
                            }
                            else
                            {
                                combined.Add(option);
                            }
                        }
                    }
                }

                navView.MenuItemsSource = combined;
            }
        }

        private bool IsSettingsItem(object menuItem)
        {
            return menuItem.GetType()
                           .GetProperties()
                           .Any(p => p.GetValue(menuItem).ToString().IndexOf("setting", StringComparison.OrdinalIgnoreCase) >= 0
                                  || p.GetValue(menuItem).ToString() == ((char)Symbol.Setting).ToString());
        }

        private void NavViewSetSelectedItem(object item)
        {
            if (UsingNavView && _navigationView is NavigationView navView)
            {
                navView.SelectedItem = item;
            }
        }

        private void NavigationViewLoaded(object sender, RoutedEventArgs e)
        {
            var navView = sender as NavigationView;
            if (navView == null)
            {
                return;
            }

            navView.Loaded -= NavigationViewLoaded;

            if (navView.FindDescendantByName("TogglePaneButton") is Button hamburgerButton)
            {
                var templateBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(HamburgerMenuTemplate)),
                    Mode = BindingMode.OneWay
                };

                var heightBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(HamburgerHeight)),
                    Mode = BindingMode.OneWay
                };

                var widthBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(HamburgerWidth)),
                    Mode = BindingMode.OneWay
                };

                var marginBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(HamburgerMargin)),
                    Mode = BindingMode.OneWay
                };

                var foregroundMargin = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(PaneForeground)),
                    Mode = BindingMode.OneWay
                };

                hamburgerButton.SetBinding(Button.ContentTemplateProperty, templateBinding);
                hamburgerButton.SetBinding(Button.HeightProperty, heightBinding);
                hamburgerButton.SetBinding(Button.WidthProperty, widthBinding);
                hamburgerButton.SetBinding(Button.MarginProperty, marginBinding);
                hamburgerButton.SetBinding(Button.ForegroundProperty, foregroundMargin);
            }
        }

        private void NavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                SelectedItem = null;
                SelectedIndex = -1;
                SelectedOptionsItem = null;
                SelectedOptionsIndex = -1;
            }
            else if (args.SelectedItem != null)
            {
                var items = ItemsSource as IEnumerable<object>;
                var options = OptionsItemsSource as IEnumerable<object>;
                if (items != null && items.Contains(args.SelectedItem))
                {
                    SelectedItem = args.SelectedItem;
                    SelectedIndex = items.ToList().IndexOf(SelectedItem);
                    SelectedOptionsItem = null;
                    SelectedOptionsIndex = -1;
                }
                else if (options != null && options.Contains(args.SelectedItem))
                {
                    SelectedItem = null;
                    SelectedIndex = -1;
                    SelectedOptionsItem = args.SelectedItem;
                    SelectedOptionsIndex = options.ToList().IndexOf(SelectedItem);
                }
            }
        }
    }
}
