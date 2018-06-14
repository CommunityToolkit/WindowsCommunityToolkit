// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class HamburgerMenuPage : IXamlRenderListener
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private HamburgerMenu hamburgerMenuControl;
        private Grid contentGrid;

        public HamburgerMenuPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            contentGrid = control.FindChildByName("ContentGrid") as Grid;
            hamburgerMenuControl = control.FindDescendantByName("HamburgerMenu") as HamburgerMenu;
            if (hamburgerMenuControl != null)
            {
                hamburgerMenuControl.ItemInvoked += HamburgerMenuControl_ItemInvoked;
            }
        }

        private async void HamburgerMenuControl_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (e.IsItemOptions)
            {
                var menuItem = e.InvokedItem as HamburgerMenuItem;
                var dialog = new MessageDialog($"You clicked on {menuItem.Label} button");

                await dialog.ShowAsync();
            }
            else if (contentGrid != null)
            {
                contentGrid.DataContext = e.InvokedItem;
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
