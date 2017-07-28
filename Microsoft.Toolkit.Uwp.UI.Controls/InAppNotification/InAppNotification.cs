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
        private Button _dismissButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotification"/> class.
        /// </summary>
        public InAppNotification()
        {
            DefaultStyleKey = typeof(InAppNotification);
        }

        protected override void OnApplyTemplate()
        {
            if (_dismissButton != null)
            {
                _dismissButton.Click -= DismissButton_Click;
            }

            _dismissButton = (Button)GetTemplateChild(DismissButtonPart);

            if (_dismissButton != null)
            {
                _dismissButton.Click += DismissButton_Click;
            }

            base.OnApplyTemplate();
        }

        public void Show()
        {
            Visibility = Visibility.Visible;
            VisualStateManager.GoToState(this, StateContentVisible, true);
        }

        public void Show(string text)
        {
            ContentTemplate = null;
            Content = text;
            Show();
        }

        public void Show(UIElement element)
        {
            ContentTemplate = null;
            Content = element;
            Show();
        }

        public void Show(DataTemplate dataTemplate)
        {
            ContentTemplate = dataTemplate;
            Content = null;
            Show();
        }

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
