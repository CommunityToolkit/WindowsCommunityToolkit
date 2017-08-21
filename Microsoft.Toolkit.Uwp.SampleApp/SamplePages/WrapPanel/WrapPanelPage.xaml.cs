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

using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// WrapPanel sample page
    /// </summary>
    public sealed partial class WrapPanelPage : Page
    {
        private static readonly Random Rand = new Random();
        private ObservableCollection<PhotoDataItemWithDimension> _wrapPanelCollection;
        private WrapPanel _sampleWrapPanel;

        public WrapPanelPage()
        {
            InitializeComponent();
            Loaded += WrapPanelPage_Loaded;
        }

        private void WrapPanelPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _wrapPanelCollection = new ObservableCollection<PhotoDataItemWithDimension>();
            WrapPanelContainer.ItemsSource = _wrapPanelCollection;
            _sampleWrapPanel = WrapPanelContainer.FindDescendant<WrapPanel>();
        }

        private void Grid_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var item = (sender as Grid)?.DataContext as PhotoDataItemWithDimension;
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

        private void SwitchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _sampleWrapPanel.Orientation = _sampleWrapPanel.Orientation == Orientation.Horizontal
                ? Orientation.Vertical
                : Orientation.Horizontal;
        }
    }
}
