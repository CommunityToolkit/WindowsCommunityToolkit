// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// WrapPanel sample page
    /// </summary>
    public sealed partial class WrapPanelPage : Page, IXamlRenderListener
    {
        private static readonly Random Rand = new Random();
        private ObservableCollection<PhotoDataItemWithDimension> _wrapPanelCollection;
        private ListView _itemControl;

        public WrapPanelPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _itemControl = control.FindChildByName("WrapPanelContainer") as ListView;

            if (_itemControl != null)
            {
                _itemControl.ItemsSource = _wrapPanelCollection;
                _itemControl.ItemClick += ItemControl_ItemClick;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _wrapPanelCollection = new ObservableCollection<PhotoDataItemWithDimension>();

            Shell.Current.RegisterNewCommand("Add random sized Image", AddButton_Click);
            Shell.Current.RegisterNewCommand("Add fixed sized Image", AddFixedButton_Click);
            Shell.Current.RegisterNewCommand("Switch Orientation", SwitchButton_Click);
        }

        private void ItemControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as PhotoDataItemWithDimension;
            if (item == null)
            {
                return;
            }

            _wrapPanelCollection.Remove(item);
        }

        private void AddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _wrapPanelCollection.Add(new PhotoDataItemWithDimension
            {
                Category = "Remove",
                Thumbnail = "ms-appx:///Assets/Photos/BigFourSummerHeat.jpg",
                Width = Rand.Next(120, 180),
                Height = Rand.Next(80, 130)
            });
        }

        private void AddFixedButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _wrapPanelCollection.Add(new PhotoDataItemWithDimension
            {
                Category = "Remove",
                Thumbnail = "ms-appx:///Assets/Photos/BigFourSummerHeat.jpg",
                Width = 150,
                Height = 100
            });
        }

        private void SwitchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_itemControl.FindDescendant<WrapPanel>() is var sampleWrapPanel)
            {
                if (sampleWrapPanel.Orientation == Orientation.Horizontal)
                {
                    sampleWrapPanel.Orientation = Orientation.Vertical;
                    ScrollViewer.SetVerticalScrollMode(_itemControl, ScrollMode.Disabled);
                    ScrollViewer.SetVerticalScrollBarVisibility(_itemControl, ScrollBarVisibility.Disabled);
                    ScrollViewer.SetHorizontalScrollMode(_itemControl, ScrollMode.Auto);
                    ScrollViewer.SetHorizontalScrollBarVisibility(_itemControl, ScrollBarVisibility.Auto);
                }
                else
                {
                    sampleWrapPanel.Orientation = Orientation.Horizontal;
                    ScrollViewer.SetVerticalScrollMode(_itemControl, ScrollMode.Auto);
                    ScrollViewer.SetVerticalScrollBarVisibility(_itemControl, ScrollBarVisibility.Auto);
                    ScrollViewer.SetHorizontalScrollMode(_itemControl, ScrollMode.Disabled);
                    ScrollViewer.SetHorizontalScrollBarVisibility(_itemControl, ScrollBarVisibility.Disabled);
                }
            }
        }
    }
}
