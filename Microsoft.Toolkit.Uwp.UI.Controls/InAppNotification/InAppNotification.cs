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
using System.Threading.Tasks;
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
        private string _id;
        private Button _dismissButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotification"/> class.
        /// </summary>
        public InAppNotification()
        {
            DefaultStyleKey = typeof(InAppNotification);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            if (_dismissButton != null)
            {
                _dismissButton.Click -= DismissButton_Click;
            }

            _dismissButton = (Button)GetTemplateChild(DismissButtonPart);

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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ShowAsync(int duration = 0)
        {
            var newId = Guid.NewGuid().ToString();
            _id = newId;

            Visibility = Visibility.Visible;
            VisualStateManager.GoToState(this, StateContentVisible, true);

            if (duration > 0)
            {
                await Task.Delay(duration);

                if (_id == newId)
                {
                    Dismiss();
                }
            }
        }

        /// <summary>
        /// Show notification using text as the content of the notification
        /// </summary>
        /// <param name="text">Text used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ShowAsync(string text, int duration = 0)
        {
            ContentTemplate = null;
            Content = text;
            return ShowAsync(duration);
        }

        /// <summary>
        /// Show notification using UIElement as the content of the notification
        /// </summary>
        /// <param name="element">UIElement used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ShowAsync(UIElement element, int duration = 0)
        {
            ContentTemplate = null;
            Content = element;
            return ShowAsync(duration);
        }

        /// <summary>
        /// Show notification using DataTemplate as the content of the notification
        /// </summary>
        /// <param name="dataTemplate">DataTemplate used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ShowAsync(DataTemplate dataTemplate, int duration = 0)
        {
            ContentTemplate = dataTemplate;
            Content = null;
            return ShowAsync(duration);
        }

        /// <summary>
        /// Dismiss the notification
        /// </summary>
        public void Dismiss()
        {
            if (Visibility == Visibility.Visible)
            {
                VisualStateManager.GoToState(this, StateContentCollapsed, true);
                Dismissed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
