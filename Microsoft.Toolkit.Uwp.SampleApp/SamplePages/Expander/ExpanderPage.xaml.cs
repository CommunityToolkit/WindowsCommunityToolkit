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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{

    public sealed partial class ExpanderPage
    {
        public ExpanderPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Expand content Down", (sender, args) =>
            {
                expander.ExpandDirection = UI.Controls.ExpandDirection.Down;
                expander.Header = "Expand Down";
                expander.VerticalAlignment = VerticalAlignment.Top;
                expander.HorizontalAlignment = HorizontalAlignment.Stretch;
            });

            Shell.Current.RegisterNewCommand("Expand content Up", (sender, args) =>
            {
                expander.ExpandDirection = UI.Controls.ExpandDirection.Up;
                expander.Header = "Expand Up";
                expander.VerticalAlignment = VerticalAlignment.Bottom;
                expander.HorizontalAlignment = HorizontalAlignment.Stretch;
            });

            Shell.Current.RegisterNewCommand("Expand content Left", (sender, args) =>
            {
                expander.ExpandDirection = UI.Controls.ExpandDirection.Left;
                expander.Header = "Expand Left";
                expander.VerticalAlignment = VerticalAlignment.Stretch;
                expander.HorizontalAlignment = HorizontalAlignment.Right;
            });

            Shell.Current.RegisterNewCommand("Expand content Right", (sender, args) =>
            {
                expander.ExpandDirection = UI.Controls.ExpandDirection.Right;
                expander.Header = "Expand Right";
                expander.VerticalAlignment = VerticalAlignment.Stretch;
                expander.HorizontalAlignment = HorizontalAlignment.Left;
            });
        }
    }
}
