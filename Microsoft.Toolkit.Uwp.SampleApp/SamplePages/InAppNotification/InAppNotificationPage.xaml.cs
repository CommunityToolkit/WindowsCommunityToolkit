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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class InAppNotificationPage : Page
    {
        public InAppNotificationPage()
        {
            InitializeComponent();
        }

        private void ShowNotificationWithRandomTextButton_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            int result = random.Next(1, 4);

            if (result == 1)
            {
                ExampleInAppNotification.Show("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sollicitudin bibendum enim at tincidunt. Praesent egestas ipsum ligula, nec tincidunt lacus semper non.");
            }

            if (result == 2)
            {
                ExampleInAppNotification.Show("Pellentesque in risus eget leo rhoncus ultricies nec id ante.");
            }

            if (result == 3)
            {
                ExampleInAppNotification.Show("Sed quis nisi quis nunc condimentum varius id consectetur metus. Duis mauris sapien, commodo eget erat ac, efficitur iaculis magna. Morbi eu velit nec massa pharetra cursus. Fusce non quam egestas leo finibus interdum eu ac massa. Quisque nec justo leo. Aenean scelerisque placerat ultrices. Sed accumsan lorem at arcu commodo tristique.");
            }
        }

        private void ShowNotificationWithButtonsButton_Click(object sender, RoutedEventArgs e)
        {
            var grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Text part
            var textBlock = new TextBlock
            {
                Text = "Do you like it?",
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(textBlock);

            // Buttons part
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            var yesButton = new Button
            {
                Content = "Yes",
                Width = 150,
                Height = 30
            };
            yesButton.Click += YesButton_Click;
            stackPanel.Children.Add(yesButton);

            var noButton = new Button
            {
                Content = "No",
                Width = 150,
                Height = 30,
                Margin = new Thickness(10, 0, 0, 0)
            };
            noButton.Click += NoButton_Click;
            stackPanel.Children.Add(noButton);

            Grid.SetColumn(stackPanel, 1);
            grid.Children.Add(stackPanel);

            ExampleInAppNotification.Show(grid);
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            ExampleInAppNotification.Dismiss();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            ExampleInAppNotification.Dismiss();
        }

        private void DismissNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            ExampleInAppNotification.Dismiss();
        }
    }
}
