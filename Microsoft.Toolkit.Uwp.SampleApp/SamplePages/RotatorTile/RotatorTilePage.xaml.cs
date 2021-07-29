// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class RotatorTilePage : IXamlRenderListener
    {
        private ObservableCollection<PhotoDataItem> _pictures;

        public RotatorTilePage()
        {
            InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var tile1 = control.FindChildByName("Tile1") as RotatorTile;

            if (tile1 != null)
            {
                tile1.ItemsSource = _pictures;
            }

            var tile2 = control.FindChildByName("Tile2") as RotatorTile;

            if (tile2 != null)
            {
                tile2.ItemsSource = _pictures;
            }

            var tile3 = control.FindChildByName("Tile3") as RotatorTile;

            if (tile3 != null)
            {
                tile3.ItemsSource = _pictures;
            }

            var tile4 = control.FindChildByName("Tile4") as RotatorTile;

            if (tile4 != null)
            {
                tile4.ItemsSource = _pictures;
            }
        }

        private async void Load()
        {
            _pictures = await new Data.PhotosDataSource().GetItemsAsync(true);
        }
    }
}
