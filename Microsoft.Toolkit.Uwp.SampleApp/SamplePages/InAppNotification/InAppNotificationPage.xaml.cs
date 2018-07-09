// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class InAppNotificationPage : Page, IXamlRenderListener
    {
        private ControlTemplate _defaultInAppNotificationControlTemplate;
        private ControlTemplate _customInAppNotificationControlTemplate;
        private InAppNotification _exampleInAppNotification;
        private InAppNotification _exampleCustomInAppNotification;
        private InAppNotification _exampleVSCodeInAppNotification;
        private ResourceDictionary _resources;

        public bool IsRootGridActualWidthLargerThan700 { get; set; }

        public int NotificationDuration { get; set; } = 0;

        public InAppNotificationPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            NotificationDuration = 0;

            _exampleInAppNotification = control.FindChildByName("ExampleInAppNotification") as InAppNotification;
            _defaultInAppNotificationControlTemplate = _exampleInAppNotification?.Template;
            _exampleCustomInAppNotification = control.FindChildByName("ExampleCustomInAppNotification") as InAppNotification;
            _customInAppNotificationControlTemplate = _exampleCustomInAppNotification?.Template;
            _exampleVSCodeInAppNotification = control.FindChildByName("ExampleVSCodeInAppNotification") as InAppNotification;
            _resources = control.Resources;

            var notificationDurationTextBox = control.FindChildByName("NotificationDurationTextBox") as TextBox;
            if (notificationDurationTextBox != null)
            {
                notificationDurationTextBox.TextChanged += NotificationDurationTextBox_TextChanged;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Show notification with random text", (sender, args) =>
            {
                _exampleVSCodeInAppNotification?.Dismiss();
                SetDefaultControlTemplate();
                _exampleInAppNotification?.Show(GetRandomText(), NotificationDuration);
            });

            Shell.Current.RegisterNewCommand("Show notification with buttons (without DataTemplate)", (sender, args) =>
            {
                _exampleVSCodeInAppNotification?.Dismiss();
                SetDefaultControlTemplate();

                var grid = new Grid()
                {
                    Margin = new Thickness(0, 0, -38, 0)
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                // Text part
                var textBlock = new TextBlock
                {
                    Text = "Do you like it?",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 24, 0),
                    FontSize = 16
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
                    Width = 120,
                    Height = 40,
                    FontSize = 16
                };
                yesButton.Click += YesButton_Click;
                stackPanel.Children.Add(yesButton);

                var noButton = new Button
                {
                    Content = "No",
                    Width = 120,
                    Height = 40,
                    FontSize = 16,
                    Margin = new Thickness(4, 0, 0, 0)
                };
                noButton.Click += NoButton_Click;
                stackPanel.Children.Add(noButton);

                Grid.SetColumn(stackPanel, 1);
                grid.Children.Add(stackPanel);

                _exampleInAppNotification?.Show(grid, NotificationDuration);
            });

            Shell.Current.RegisterNewCommand("Show notification with buttons (with DataTemplate)", (sender, args) =>
            {
                _exampleVSCodeInAppNotification?.Dismiss();
                SetCustomControlTemplate(); // Use the custom template without the Dismiss button. The DataTemplate will handle readding it.

                object inAppNotificationWithButtonsTemplate = null;
                bool? isTemplatePresent = _resources?.TryGetValue("InAppNotificationWithButtonsTemplate", out inAppNotificationWithButtonsTemplate);

                if (isTemplatePresent == true && inAppNotificationWithButtonsTemplate is DataTemplate template)
                {
                    _exampleInAppNotification.Show(template, NotificationDuration);
                }
            });

            Shell.Current.RegisterNewCommand("Show notification with Drop Shadow (based on default template)", (sender, args) =>
            {
                _exampleVSCodeInAppNotification.Dismiss();
                SetDefaultControlTemplate();

                // Update control template
                object inAppNotificationDropShadowControlTemplate = null;
                bool? isTemplatePresent = _resources?.TryGetValue("InAppNotificationDropShadowControlTemplate", out inAppNotificationDropShadowControlTemplate);

                if (isTemplatePresent == true && inAppNotificationDropShadowControlTemplate is ControlTemplate template)
                {
                    _exampleInAppNotification.Template = template;
                }

                _exampleInAppNotification.Show(GetRandomText(), NotificationDuration);
            });

            Shell.Current.RegisterNewCommand("Show notification with Visual Studio Code template (info notification)", (sender, args) =>
            {
                _exampleInAppNotification.Dismiss();
                _exampleVSCodeInAppNotification.Show(NotificationDuration);
            });

            Shell.Current.RegisterNewCommand("Dismiss", (sender, args) =>
            {
                // Dismiss all notifications (should not be replicated in production)
                _exampleInAppNotification.Dismiss();
                _exampleVSCodeInAppNotification.Dismiss();
            });
        }

        private string GetRandomText()
        {
            var random = new Random();
            int result = random.Next(1, 4);

            switch (result)
            {
                case 1: return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sollicitudin bibendum enim at tincidunt. Praesent egestas ipsum ligula, nec tincidunt lacus semper non.";
                case 2: return "Pellentesque in risus eget leo rhoncus ultricies nec id ante.";
                case 3: default: return "Sed quis nisi quis nunc condimentum varius id consectetur metus. Duis mauris sapien, commodo eget erat ac, efficitur iaculis magna. Morbi eu velit nec massa pharetra cursus. Fusce non quam egestas leo finibus interdum eu ac massa. Quisque nec justo leo. Aenean scelerisque placerat ultrices. Sed accumsan lorem at arcu commodo tristique.";
            }
        }

        private void SetDefaultControlTemplate()
        {
            // Update control template
            _exampleInAppNotification.Template = _defaultInAppNotificationControlTemplate;
        }

        private void SetCustomControlTemplate()
        {
            // Update control template
            _exampleInAppNotification.Template = _customInAppNotificationControlTemplate;
        }

        private void NotificationDurationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newDuration;
            if (int.TryParse((sender as TextBox)?.Text, out newDuration))
            {
                NotificationDuration = newDuration;
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            _exampleInAppNotification?.Dismiss();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            _exampleInAppNotification?.Dismiss();
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    internal class DismissCommand : ICommand
#pragma warning restore SA1402 // File may only contain a single class
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            (parameter as InAppNotification)?.Dismiss();
        }
    }
}
