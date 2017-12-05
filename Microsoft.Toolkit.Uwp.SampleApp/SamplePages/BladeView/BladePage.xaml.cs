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

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An page that shows how to use the Blade Control
    /// </summary>
    public sealed partial class BladePage : IXamlRenderListener
    {
        private BladeView bladeView;
        private Button addBlade;
        private ResourceDictionary resources;

        public BladePage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            bladeView = control.FindChildByName("BladeView") as BladeView;
            addBlade = control.FindChildByName("AddBlade") as Button;

            if (addBlade != null)
            {
                addBlade.Click += OnAddBladeButtonClicked;
            }

            resources = control.Resources;
        }

        private void OnAddBladeButtonClicked(object sender, RoutedEventArgs e)
        {
            if (resources?.ContainsKey("BladeStyle") == true)
            {
                BladeItem bladeItem = new BladeItem()
                {
                    Style = resources["BladeStyle"] as Style
                };

                bladeView?.Items?.Add(bladeItem);
            }
        }
    }
}