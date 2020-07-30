// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
#if WINDOWS_UWP
using Microsoft.UI.Xaml.Input;
#else
using System.Windows.Input;
#endif
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ListViewExtensionsPage : Page, IXamlRenderListener
    {
        public ListViewExtensionsPage()
        {
            this.InitializeComponent();
        }

        public ICommand SampleCommand => new DelegateCommand<PhotoDataItem>(OnExecuteSampleCommand);

        public async void OnXamlRendered(FrameworkElement control)
        {
            var sampleListView = control.FindChildByName("SampleListView") as ListView;

            if (sampleListView != null)
            {
                sampleListView.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
            }

            // Transfer Data Context so we can access SampleCommand
            control.DataContext = this;
        }

        private async void OnExecuteSampleCommand(PhotoDataItem item)
        {
            await new ContentDialog
            {
                Title = "Item Clicked",
                Content = $"You clicked {item.Title} via the 'ListViewExtensions.Command' binding",
                CloseButtonText = "Close",
                XamlRoot = XamlRoot
            }.ShowAsync();
        }
    }
}
