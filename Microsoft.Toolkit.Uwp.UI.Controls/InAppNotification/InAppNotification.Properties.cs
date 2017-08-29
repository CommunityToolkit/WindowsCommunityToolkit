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

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// In App Notification defines a control to show local notification in the app.
    /// </summary>
    public partial class InAppNotification
    {
        /// <summary>
        /// Identifies the <see cref="ShowDismissButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowDismissButtonProperty =
            DependencyProperty.Register(nameof(ShowDismissButton), typeof(bool), typeof(InAppNotification), new PropertyMetadata(true, OnShowDismissButtonChanged));

        /// <summary>
        /// Gets or sets a value indicating whether to show the Dismiss button of the control.
        /// </summary>
        public bool ShowDismissButton
        {
            get { return (bool)GetValue(ShowDismissButtonProperty); }
            set { SetValue(ShowDismissButtonProperty, value); }
        }

        private static void OnShowDismissButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inApNotification = d as InAppNotification;

            bool showDismissButton = (bool)e.NewValue;

            if (inApNotification._dismissButton != null)
            {
                inApNotification._dismissButton.Visibility = showDismissButton ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
