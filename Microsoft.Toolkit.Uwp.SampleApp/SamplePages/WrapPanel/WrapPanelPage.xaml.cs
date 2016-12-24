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
using Microsoft.Toolkit.Uwp.UI.Controls.WrapPanel;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// WrapPanel sample page
    /// </summary>
    public sealed partial class WrapPanelPage : Page
    {
        private static readonly Random Rand = new Random();

        private static Button GenerateButton()
        {
            var button = new Button { Content = "Remove", Margin = new Windows.UI.Xaml.Thickness(3), Width = (double)Rand.Next(20, 80), Height = (double)Rand.Next(20, 80) };
            button.Click += (sender, args) =>
            {
                var currentButton = sender as Button;
                var parent = currentButton?.Parent as WrapPanel;
                parent?.Children.Remove(button);
            };
            return button;
        }

        public WrapPanelPage()
        {
            InitializeComponent();
        }

        private void HorizontalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            HorizontalWrapPanel.Children.Add(GenerateButton());
        }

        private void VerticalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            VerticalWrapPanel.Children.Add(GenerateButton());
        }
    }
}
