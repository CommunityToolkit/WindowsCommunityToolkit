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

using Microsoft.Toolkit.Uwp.UI.Extensions;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenu is based on a SplitView control. By default it contains a HamburgerButton and a ListView to display menu items.
    /// </summary>
    [TemplatePart(Name = "HamburgerButton", Type = typeof(Button))]
    [TemplatePart(Name = "ButtonsListView", Type = typeof(Windows.UI.Xaml.Controls.ListViewBase))]
    [TemplatePart(Name = "OptionsListView", Type = typeof(Windows.UI.Xaml.Controls.ListViewBase))]
    public partial class HamburgerMenu : ContentControl
    {
        private Button _hamburgerButton;
        private Windows.UI.Xaml.Controls.ListViewBase _buttonsListView;
        private Windows.UI.Xaml.Controls.ListViewBase _optionsListView;

        private object _navigationView;
        private Button _navViewHamburgerButton;
        private bool temp = true; // TODO REPLACE ME WITH REAL CHECK

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
            if (!temp && PaneForeground == null)
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

            if (_navigationView != null && temp)
            {
                if (_navigationView is NavigationView navView)
                {
                    navView.ItemInvoked -= NavView_ItemInvoked;
                }
            }

            _hamburgerButton = (Button)GetTemplateChild("HamburgerButton");
            _buttonsListView = (Windows.UI.Xaml.Controls.ListViewBase)GetTemplateChild("ButtonsListView");
            _optionsListView = (Windows.UI.Xaml.Controls.ListViewBase)GetTemplateChild("OptionsListView");

            if (temp)
            {
                _navigationView = (NavigationView)GetTemplateChild("NavView");
                if (_navigationView is NavigationView navView)
                {
                    navView.ItemInvoked += NavView_ItemInvoked;
                    navView.MenuItemTemplateSelector = new HamburgerMenuNavViewItemTemplateSelector(this);
                    OnItemsSourceChanged(this, null);

                    _navViewHamburgerButton = navView.FindChildByName("TogglePaneButton") as Button;

                    if (_navViewHamburgerButton != null)
                    {
                        // subscribe events
                        if (PaneForeground != null)
                        {
                            _navViewHamburgerButton.Foreground = PaneForeground;
                        }

                        if (HamburgerMenuTemplate != null)
                        {
                            _navViewHamburgerButton.ContentTemplate = HamburgerMenuTemplate;
                        }
                    }
                }
            }

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

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hm = d as HamburgerMenu;

            if (hm.temp && hm._navigationView is NavigationView navView)
            {
                var items = hm.ItemsSource as IEnumerable<object>;
                var options = hm.OptionsItemsSource as IEnumerable<object>;

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
                        combined.Add(option);
                    }
                }

                navView.MenuItemsSource = combined;
            }
        }
    }
}
