// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ReorderGridPage : Page, IXamlRenderListener
    {
        private GridView imageView;

        public ReorderGridPage()
        {
            InitializeComponent();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            imageView = control.FindChildByName("ImageView") as GridView;

            if (imageView != null)
            {
                imageView.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
            }
        }
    }
}
