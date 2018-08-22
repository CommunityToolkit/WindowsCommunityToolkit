// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TabViewPage : Page, IXamlRenderListener
    {
        public TabViewPage()
        {
            this.InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var tabs = control.FindChildByName("Tabs") as TabView;
            if (tabs != null)
            {
                tabs.TabDraggedOutside += Tabs_TabDraggedOutside;
                tabs.TabClosing += Tabs_TabClosing;
            }
        }

        private void Tabs_TabClosing(object sender, TabClosingEventArgs e)
        {
            if (e.Tab.Header.ToString() == "Not Closable")
            {
                e.Cancel = true;
            }

            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            new MessageDialog("You're closing the '" + e.Tab.Header + "' tab.").ShowAsync();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private async void Tabs_TabDraggedOutside(object sender, TabDraggedOutsideEventArgs e)
        {
            await new MessageDialog("Tore Tab Outside App.  TODO: Pop-open a Window.").ShowAsync();
        }
    }
}
