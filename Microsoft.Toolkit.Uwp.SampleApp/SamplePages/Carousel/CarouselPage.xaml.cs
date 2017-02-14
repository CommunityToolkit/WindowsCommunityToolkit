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

using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Data;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.SampleApp.Models;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class CarouselPage : Page
    {
        private ObservableCollection<PhotoDataItem> items = new ObservableCollection<PhotoDataItem>();

        private bool test { get; set; } = false;

        public CarouselPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            foreach (var photo in await new Data.PhotosDataSource().GetItemsAsync())
            {
                items.Add(photo);
            }
        }

        private void CarouselItem_CarouselItemLocationChanged(object sender, CarouselItemLocationChangedEventArgs e)
        {
            var item = sender as CarouselItem;
            float centerX = (float)item.ActualWidth / 2;
            float centerY = (float)item.ActualHeight / 2;
            TextBlock text = null;

            IDictionary<string, object> expando = DataContext as ExpandoObject;
            var animationsEnabled = (bool)(expando["DefaultAnimationEnabled"] as ValueHolder).Value;

            if (item.DefaultAnimationEnabled && animationsEnabled)
            {
                return;
            }
            else if (item.DefaultAnimationEnabled && !animationsEnabled)
            {
                item.DefaultAnimationEnabled = false;
            }
            else if (!item.DefaultAnimationEnabled && animationsEnabled)
            {
                item.DefaultAnimationEnabled = true;
                item.Rotate(0, centerX, centerY, 200).Start();
                text = item.FindName("Text") as TextBlock;
                if (text != null)
                {
                    text.Scale(scaleX: 1, scaleY: 1, centerX: centerX).Start();
                    text.Offset(offsetY: 0, offsetX: 0).Start();
                }

                 return;
            }

            item.Rotate(item.CarouselItemLocation * 10f, centerX, centerY, 200).Start();

            text = item.FindName("Text") as TextBlock;
            if (text != null)
            {
                centerX = (float)text.ActualWidth / 2;
                if (item.CarouselItemLocation == 0)
                {
                    if (CarouselControl.Orientation == Orientation.Horizontal)
                    {
                        text.Scale(scaleX: 2, scaleY: 2, centerX: centerX).Start();
                        text.Offset(offsetY: 200).Start();
                    }
                    else
                    {
                        text.Scale(scaleX: 2, scaleY: 2).Start();
                        text.Offset(offsetX: 200, offsetY: -90).Start();
                    }
                }
                else
                {
                    text.Scale(scaleX: 1, scaleY: 1, centerX: centerX).Start();
                    text.Offset(offsetY: 0, offsetX: 0).Start();
                }
            }
        }
    }
}
