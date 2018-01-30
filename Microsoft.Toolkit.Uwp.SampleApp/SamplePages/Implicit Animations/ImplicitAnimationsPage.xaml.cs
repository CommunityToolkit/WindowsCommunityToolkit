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
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImplicitAnimationsPage : IXamlRenderListener
    {
        private Random _random = new Random();
        private UIElement _element;

        public ImplicitAnimationsPage()
        {
            this.InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            _element = control.FindChildByName("Element");
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Toggle Visibility", (sender, args) =>
            {
                if (_element != null)
                {
                    _element.Visibility = _element.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                }
            });

            SampleController.Current.RegisterNewCommand("Move Element", (sender, args) =>
            {
                if (_element != null)
                {
                    Canvas.SetTop(_element, _random.NextDouble() * this.ActualHeight);
                    Canvas.SetLeft(_element, _random.NextDouble() * this.ActualWidth);
                }
            });

            SampleController.Current.RegisterNewCommand("Scale Element", (sender, args) =>
            {
                if (_element != null)
                {
                    var visual = ElementCompositionPreview.GetElementVisual(_element);
                    visual.Scale = new Vector3(
                        (float)_random.NextDouble() * 2,
                        (float)_random.NextDouble() * 2,
                        1);
                }
            });
        }
    }
}