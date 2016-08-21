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
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class AdaptiveGridViewPage
    {
        public AdaptiveGridViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            AdaptiveGridViewControl.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
            AdaptiveGridViewControl.RegisterPropertyChangedCallback(AdaptiveGridView.ItemAspectRatioProperty, OnAspectRatioChanged);
        }

        private void OnAspectRatioChanged(DependencyObject sender, DependencyProperty dp)
        {
            // If the user clears the aspect ratio, then let's set the ItemHeight back to its default value.
            if (AdaptiveGridViewControl.ItemAspectRatio == 0)
            {
                AdaptiveGridViewControl.ItemHeight = 200;
            }
        }
    }
}
