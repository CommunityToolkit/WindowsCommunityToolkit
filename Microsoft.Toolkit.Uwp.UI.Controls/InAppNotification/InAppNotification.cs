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
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// In App Notification defines a control to show local notification in the app.
    /// </summary>
    [TemplateVisualState(Name = StateContentVisible, GroupName = GroupContent)]
    [TemplateVisualState(Name = StateContentCollapsed, GroupName = GroupContent)]
    [TemplatePart(Name = DismissButtonPart, Type = typeof(Button))]
    public sealed partial class InAppNotification : ContentControl
    {
        private InAppNotificationDismissKind _lastDismissKind;
        private DispatcherTimer _animationTimer = new DispatcherTimer();
        private DispatcherTimer _dismissTimer = new DispatcherTimer();
        private Button _dismissButton;
        private VisualStateGroup _visualStateGroup;
        private List<StackedNotificationInfo> _stackedNotificationInfos = new List<StackedNotificationInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotification"/> class.
        /// </summary>
        public InAppNotification()
        {
            DefaultStyleKey = typeof(InAppNotification);

            _dismissTimer.Tick += DismissTimer_Tick;
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            if (_dismissButton != null)
            {
                _dismissButton.Click -= DismissButton_Click;
            }

            _dismissButton = (Button)GetTemplateChild(DismissButtonPart);
            _visualStateGroup = (VisualStateGroup)GetTemplateChild(GroupContent);

            if (_dismissButton != null)
            {
                _dismissButton.Visibility = ShowDismissButton ? Visibility.Visible : Visibility.Collapsed;
                _dismissButton.Click += DismissButton_Click;
            }

            if (Visibility == Visibility.Visible)
            {
                VisualStateManager.GoToState(this, StateContentVisible, true);
            }
            else
            {
                VisualStateManager.GoToState(this, StateContentCollapsed, true);
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Show notification using the current template
        /// </summary>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(int duration = 0)
        {
            _animationTimer.Stop();
            _dismissTimer.Stop();

            var eventArgs = new InAppNotificationOpeningEventArgs();
            Opening?.Invoke(this, eventArgs);

            if (eventArgs.Cancel)
            {
                return;
            }

            Visibility = Visibility.Visible;
            VisualStateManager.GoToState(this, StateContentVisible, true);

            _animationTimer.Interval = AnimationDuration;
            _animationTimer.Tick += OpenAnimationTimer_Tick;
            _animationTimer.Start();

            if (duration > 0)
            {
                _dismissTimer.Interval = TimeSpan.FromMilliseconds(duration);
                _dismissTimer.Start();
            }
        }

        /// <summary>
        /// Show notification using text as the content of the notification
        /// </summary>
        /// <param name="text">Text used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(string text, int duration = 0)
        {
            var notificationInfo = new TextStackedNotificationInfo
            {
                Duration = duration,
                Text = text
            };
            Show(notificationInfo);
        }

        /// <summary>
        /// Show notification using UIElement as the content of the notification
        /// </summary>
        /// <param name="element">UIElement used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(UIElement element, int duration = 0)
        {
            var notificationInfo = new UIElementStackedNotificationInfo
            {
                Duration = duration,
                Element = element
            };
            Show(notificationInfo);
        }

        /// <summary>
        /// Show notification using DataTemplate as the content of the notification
        /// </summary>
        /// <param name="dataTemplate">DataTemplate used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(DataTemplate dataTemplate, int duration = 0)
        {
            var notificationInfo = new DataTemplateStackedNotificationInfo
            {
                Duration = duration,
                DataTemplate = dataTemplate
            };
            Show(notificationInfo);
        }

        /// <summary>
        /// Dismiss the notification
        /// </summary>
        public void Dismiss()
        {
            Dismiss(InAppNotificationDismissKind.Programmatic);
        }

        /// <summary>
        /// Dismiss the notification
        /// </summary>
        /// <param name="dismissKind">Kind of action that triggered dismiss event</param>
        private void Dismiss(InAppNotificationDismissKind dismissKind)
        {
            if (Visibility == Visibility.Visible)
            {
                // Continue to display notification if on remaining stacked notification
                if (_stackedNotificationInfos.Any())
                {
                    _stackedNotificationInfos.RemoveAt(0);

                    if (_stackedNotificationInfos.Any())
                    {
                        DisplayNextStackedNotification(_stackedNotificationInfos[0]);
                        return;
                    }
                }

                _animationTimer.Stop();

                var closingEventArgs = new InAppNotificationClosingEventArgs(dismissKind);
                Closing?.Invoke(this, closingEventArgs);

                if (closingEventArgs.Cancel)
                {
                    return;
                }

                VisualStateManager.GoToState(this, StateContentCollapsed, true);

                _lastDismissKind = dismissKind;

                _animationTimer.Interval = AnimationDuration;
                _animationTimer.Tick += DismissAnimationTimer_Tick;
                _animationTimer.Start();
            }
        }

        /// <summary>
        /// Informs if the notification should be displayed immediately (based on the StackMode)
        /// </summary>
        /// <returns>True if notification should be displayed immediately</returns>
        private bool ShouldDisplayImmediately()
        {
            return StackMode != StackMode.QueueBehind ||
                (StackMode == StackMode.QueueBehind && _stackedNotificationInfos.Count == 0);
        }

        /// <summary>
        /// Display the next stacked notification using StackedNotificationInfo
        /// </summary>
        /// <param name="stackedNotificationInfo">Information to display for the next notification</param>
        private void DisplayNextStackedNotification(StackedNotificationInfo stackedNotificationInfo)
        {
            UpdateContent(stackedNotificationInfo);

            _dismissTimer.Stop();

            if (stackedNotificationInfo.Duration > 0)
            {
                _dismissTimer.Interval = TimeSpan.FromMilliseconds(stackedNotificationInfo.Duration);
                _dismissTimer.Start();
            }
        }

        /// <summary>
        /// Update the Content of the notification
        /// </summary>
        /// <param name="stackedNotificationInfo">Information about the notification to display</param>
        private void UpdateContent(StackedNotificationInfo stackedNotificationInfo)
        {
            switch (stackedNotificationInfo)
            {
                case TextStackedNotificationInfo textStackedNotificationInfo:
                    ContentTemplate = null;
                    Content = textStackedNotificationInfo.Text;
                    break;
                case UIElementStackedNotificationInfo UIElementStackedNotificationInfo:
                    ContentTemplate = null;
                    Content = UIElementStackedNotificationInfo.Element;
                    break;
                case DataTemplateStackedNotificationInfo dataTemplateStackedNotificationInfo:
                    ContentTemplate = dataTemplateStackedNotificationInfo.DataTemplate;
                    Content = null;
                    break;
            }
        }

        /// <summary>
        /// Handle the display of the notification based on the current StackMode
        /// </summary>
        /// <param name="stackedNotificationInfo">Information about the notification to display</param>
        private void Show(StackedNotificationInfo stackedNotificationInfo)
        {
            bool shouldDisplayImmediately = ShouldDisplayImmediately();

            if (StackMode == StackMode.QueueBehind)
            {
                _stackedNotificationInfos.Add(stackedNotificationInfo);
            }

            if (StackMode == StackMode.StackAbove)
            {
                _stackedNotificationInfos.Insert(0, stackedNotificationInfo);
            }

            if (shouldDisplayImmediately)
            {
                UpdateContent(stackedNotificationInfo);
                Show(stackedNotificationInfo.Duration);
            }
        }
    }
}
