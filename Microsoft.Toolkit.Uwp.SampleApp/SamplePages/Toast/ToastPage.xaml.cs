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

using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using NotificationsVisualizerLibrary;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ToastPage : Page
    {
        private ToastContent _toastContent;

        public ToastPage()
        {
            InitializeComponent();
            Initialize();
        }

        public static ToastContent GenerateToastContent()
        {
            return new ToastContent()
            {
                Launch = "action=viewEvent&eventId=1983",
                Scenario = ToastScenario.Reminder,

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Adaptive Tiles Meeting"
                            },

                            new AdaptiveText()
                            {
                                Text = "Conf Room 2001 / Building 135"
                            },

                            new AdaptiveText()
                            {
                                Text = "10:00 AM - 10:30 AM"
                            }
                        }
                    }
                },

                Actions = new ToastActionsCustom()
                {
                    Inputs =
                    {
                        new ToastSelectionBox("snoozeTime")
                        {
                            DefaultSelectionBoxItemId = "15",
                            Items =
                            {
                                new ToastSelectionBoxItem("1", "1 minute"),
                                new ToastSelectionBoxItem("15", "15 minutes"),
                                new ToastSelectionBoxItem("60", "1 hour"),
                                new ToastSelectionBoxItem("240", "4 hours"),
                                new ToastSelectionBoxItem("1440", "1 day")
                            }
                        }
                    },

                    Buttons =
                    {
                        new ToastButtonSnooze()
                        {
                            SelectionBoxId = "snoozeTime"
                        },

                        new ToastButtonDismiss()
                    }
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }
        }

        private void ButtonPopToast_Click(object sender, RoutedEventArgs e)
        {
            PopToast();
        }

        private void PopToast()
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(_toastContent.GetXml()));
        }

        private void Initialize()
        {
            // Generate the toast notification content
            _toastContent = GenerateToastContent();

            // Prepare and update preview toast
            PreviewToastReminder.Properties = new PreviewToastProperties()
            {
                BackgroundColor = Constants.ApplicationBackgroundColor,
                DisplayName = Constants.ApplicationDisplayName,
                Square44x44Logo = Constants.Square44x44Logo
            };
            PreviewToastReminder.Initialize(_toastContent.GetXml());
        }
    }
}
