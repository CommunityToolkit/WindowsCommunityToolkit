// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class AdaptiveGridViewPage : IXamlRenderListener
    {
        private AdaptiveGridView adaptiveGridViewControl;

        public AdaptiveGridViewPage()
        {
            InitializeComponent();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            adaptiveGridViewControl = control.FindDescendantByName("AdaptiveGridViewcontrol") as AdaptiveGridView;
            if (adaptiveGridViewControl != null)
            {
                adaptiveGridViewControl.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
                adaptiveGridViewControl.ItemClick += AdaptiveGridViewControl_ItemClick;
                adaptiveGridViewControl.SelectionChanged += AdaptiveGridViewControl_SelectionChanged;
            }
        }

        private void AdaptiveGridViewControl_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            SelectedItemCountTextBlock.Text = adaptiveGridViewControl.SelectedItems.Any()
                ? $"You have selected {adaptiveGridViewControl.SelectedItems.Count} items."
                : "You haven't selected any items";
        }

        private async void AdaptiveGridViewControl_ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                await new MessageDialog($"You clicked {(e.ClickedItem as PhotoDataItem).Title}", "Item Clicked").ShowAsync();
            }
        }
    }
}
