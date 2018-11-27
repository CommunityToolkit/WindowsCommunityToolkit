// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ListViewExtensionsPage : Page, IXamlRenderListener
    {
        public ListViewExtensionsPage()
        {
            this.InitializeComponent();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            var sampleListView = control.FindChildByName("SampleListView") as ListView;

            if (sampleListView != null)
            {
                sampleListView.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
            }
        }
    }
}
