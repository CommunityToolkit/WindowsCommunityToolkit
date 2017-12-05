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
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class OrbitViewPage : Page, IXamlRenderListener
    {
        private Random _random = new Random();

        public ObservableCollection<DeviceItem> DeviceList { get; private set; } = new ObservableCollection<DeviceItem>();

        public OrbitViewPage()
        {
            this.InitializeComponent();

            DeviceList.Add(new DeviceItem() { Distance = 0.1, Label = "My Phone", Symbol = Symbol.CellPhone });
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var people = control.FindChildByName("People") as OrbitView;
            if (people != null)
            {
                people.ItemClick += People_ItemClick;
            }

            var devices = control.FindChildByName("Devices") as OrbitView;
            if (devices != null)
            {
                devices.ItemsSource = DeviceList;
                devices.ItemClick += Devices_ItemClick;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Add Device", AddDeviceClick);
        }

        private void AddDeviceClick(object sender, RoutedEventArgs e)
        {
            switch (_random.Next(3))
            {
                case 0:
                    DeviceList.Add(new DeviceItem() { Distance = _random.Next(1, 10) / 10f, Label = "Other Phone", Symbol = Symbol.CellPhone });
                    break;
                case 1:
                    DeviceList.Add(new DeviceItem() { Distance = _random.Next(1, 10) / 10f, Label = "Camera", Symbol = Symbol.Camera });
                    break;
                case 2:
                    DeviceList.Add(new DeviceItem() { Distance = _random.Next(1, 10) / 10f, Label = "TV", Symbol = Symbol.GoToStart });
                    break;
            }
        }

        private async void People_ItemClick(object sender, OrbitViewItemClickedEventArgs e)
        {
            await new MessageDialog("You clicked: " + (e.Item as OrbitViewDataItem)?.Label).ShowAsync();
        }

        private void Devices_ItemClick(object sender, OrbitViewItemClickedEventArgs e)
        {
            DeviceList.Remove(e.Item as DeviceItem);
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class DeviceItem : OrbitViewDataItem
#pragma warning restore SA1402 // File may only contain a single class
    {
        public Symbol Symbol { get; set; }
    }
}
