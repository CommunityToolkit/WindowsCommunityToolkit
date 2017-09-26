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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class OrbitViewPage : Page
    {
        private ObservableCollection<DeviceItem> _devices = new ObservableCollection<DeviceItem>();
        private Random _random = new Random();

        public OrbitViewPage()
        {
            this.InitializeComponent();

            _devices.Add(new DeviceItem() { Distance = 0.1, Label = "My Phone", Symbol = Symbol.CellPhone });
        }

        private void AddDeviceClick(object sender, RoutedEventArgs e)
        {
            _devices.Add(new DeviceItem() { Distance = _random.Next(1, 10) / 10f, Label = "My Phone", Symbol = Symbol.CellPhone });
        }

        private void OnItemClicked(object sender, OrbitViewItemClickedEventArgs e)
        {
            _devices.Remove(e.Item as DeviceItem);
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class DeviceItem : OrbitViewDataItem
#pragma warning restore SA1402 // File may only contain a single class
    {
        public Symbol Symbol { get; set; }
    }
}
