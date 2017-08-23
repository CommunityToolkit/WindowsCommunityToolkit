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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class InAppNotificationPage : Page, INotifyPropertyChanged
    {
        private ControlTemplate _defaultInAppNotificationControlTemplate;

        public bool IsRootGridActualWidthLargerThan700 { get; set; }

        public int NotificationDuration { get; set; } = 0;

        public InAppNotificationPage()
        {
            InitializeComponent();

            _defaultInAppNotificationControlTemplate = ExampleInAppNotification.Template;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Show notification with random text", async (sender, args) =>
            {
                ExampleVSCodeInAppNotification.Dismiss();
                SetDefaultControlTemplate();

                var random = new Random();
                int result = random.Next(1, 4);

                if (result == 1)
                {
                    await ExampleInAppNotification.ShowAsync("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sollicitudin bibendum enim at tincidunt. Praesent egestas ipsum ligula, nec tincidunt lacus semper non.", NotificationDuration);
                }

                if (result == 2)
                {
                    await ExampleInAppNotification.ShowAsync("Pellentesque in risus eget leo rhoncus ultricies nec id ante.", NotificationDuration);
                }

                if (result == 3)
                {
                    await ExampleInAppNotification.ShowAsync("Sed quis nisi quis nunc condimentum varius id consectetur metus. Duis mauris sapien, commodo eget erat ac, efficitur iaculis magna. Morbi eu velit nec massa pharetra cursus. Fusce non quam egestas leo finibus interdum eu ac massa. Quisque nec justo leo. Aenean scelerisque placerat ultrices. Sed accumsan lorem at arcu commodo tristique.", NotificationDuration);
                }
            });

            Shell.Current.RegisterNewCommand("Show notification with buttons (without DataTemplate)", async (sender, args) =>
            {
                ExampleVSCodeInAppNotification.Dismiss();
                SetDefaultControlTemplate();

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

                await ExampleInAppNotification.ShowAsync(grid, NotificationDuration);
            });

            Shell.Current.RegisterNewCommand("Show notification with buttons (with DataTemplate)", async (sender, args) =>
            {
                ExampleVSCodeInAppNotification.Dismiss();
                SetDefaultControlTemplate();

                object inAppNotificationWithButtonsTemplate;
                bool isTemplatePresent = Resources.TryGetValue("InAppNotificationWithButtonsTemplate", out inAppNotificationWithButtonsTemplate);

                if (isTemplatePresent && inAppNotificationWithButtonsTemplate is DataTemplate)
                {
                    await ExampleInAppNotification.ShowAsync(inAppNotificationWithButtonsTemplate as DataTemplate, NotificationDuration);
                }
            });

            Shell.Current.RegisterNewCommand("Show notification with Drop Shadow (based on default template)", async (sender, args) =>
            {
                ExampleVSCodeInAppNotification.Dismiss();

                // Update control template
                object inAppNotificationDropShadowControlTemplate;
                bool isTemplatePresent = Resources.TryGetValue("InAppNotificationDropShadowControlTemplate", out inAppNotificationDropShadowControlTemplate);

                if (isTemplatePresent && inAppNotificationDropShadowControlTemplate is ControlTemplate)
                {
                    ExampleInAppNotification.Template = inAppNotificationDropShadowControlTemplate as ControlTemplate;
                }

                await ExampleInAppNotification.ShowAsync(NotificationDuration);
            });

            Shell.Current.RegisterNewCommand("Show notification with Visual Studio Code template (info notification)", async (sender, args) =>
            {
                ExampleInAppNotification.Dismiss();
                await ExampleVSCodeInAppNotification.ShowAsync(NotificationDuration);
            });

            Shell.Current.RegisterNewCommand("Dismiss", (sender, args) =>
            {
                // Dismiss all notifications (should not be replicated in production)
                ExampleInAppNotification.Dismiss();
                ExampleVSCodeInAppNotification.Dismiss();
            });
        }

        private void SetDefaultControlTemplate()
        {
            // Update control template
            ExampleInAppNotification.Template = _defaultInAppNotificationControlTemplate;
        }

        private void NotificationDurationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newDuration;
            if (int.TryParse(NotificationDurationTextBox.Text, out newDuration))
            {
                NotificationDuration = newDuration;
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            ExampleInAppNotification.Dismiss();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            ExampleInAppNotification.Dismiss();
        }

        private void Action1Button_Click(object sender, RoutedEventArgs e)
        {
            ExampleVSCodeInAppNotification.Dismiss();
        }

        private void Action2Button_Click(object sender, RoutedEventArgs e)
        {
            ExampleVSCodeInAppNotification.Dismiss();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ExampleVSCodeInAppNotification.Dismiss();
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // When the root part size of the In App Notification template changed, we should apply VisualState
            bool newValue = e.NewSize.Width > 700;

            if (IsRootGridActualWidthLargerThan700 != newValue)
            {
                IsRootGridActualWidthLargerThan700 = newValue;
                OnPropertyChanged(nameof(IsRootGridActualWidthLargerThan700));
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
