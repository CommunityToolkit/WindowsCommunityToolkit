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
using Windows.UI.Xaml.Media.Animation;

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
            ContentTemplate = null;
            Content = text;
            Show(duration);
        }

        /// <summary>
        /// Show notification using UIElement as the content of the notification
        /// </summary>
        /// <param name="element">UIElement used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(UIElement element, int duration = 0)
        {
            ContentTemplate = null;
            Content = element;
            Show(duration);
        }

        /// <summary>
        /// Show notification using DataTemplate as the content of the notification
        /// </summary>
        /// <param name="dataTemplate">DataTemplate used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(DataTemplate dataTemplate, int duration = 0)
        {
            ContentTemplate = dataTemplate;
            Content = null;
            Show(duration);
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
    }
}
