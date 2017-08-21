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
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A custom AppBarButton with a badge to indicate a number.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.AppBarButton" />
    public sealed class AppBarBadgeButton : AppBarButton
    {
        public AppBarBadgeButton()
        {
            this.DefaultStyleKey = typeof(AppBarBadgeButton);
        }

        public string Count
        {
            get { return (string)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(string), typeof(AppBarBadgeButton), new PropertyMetadata("0", OnCountChanged));

        private static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int count = 0;
            int.TryParse(e.NewValue.ToString(), out count);
            if (count != 0)
            {
                ((AppBarBadgeButton)d).SetValue(BadgeVisibilityProperty, Visibility.Visible);
            }
            else
            {
                ((AppBarBadgeButton)d).SetValue(BadgeVisibilityProperty, Visibility.Collapsed);
            }
        }

        public Visibility BadgeVisibility
        {
            get { return (Visibility)GetValue(BadgeVisibilityProperty); }
            set { SetValue(BadgeVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BadgeVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeVisibilityProperty =
            DependencyProperty.Register("BadgeVisibility", typeof(Visibility), typeof(AppBarBadgeButton), new PropertyMetadata(Visibility.Collapsed, null));

        public SolidColorBrush BadgeBackground
        {
            get { return (SolidColorBrush)GetValue(BadgeBackgroundProperty); }
            set { SetValue(BadgeBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BadgeBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeBackgroundProperty =
            DependencyProperty.Register("BadgeBackground", typeof(SolidColorBrush), typeof(AppBarBadgeButton), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public SolidColorBrush BadgeForeground
        {
            get { return (SolidColorBrush)GetValue(BadgeForegroundProperty); }
            set { SetValue(BadgeForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BadgeForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeForegroundProperty =
            DependencyProperty.Register("BadgeForeground", typeof(SolidColorBrush), typeof(AppBarBadgeButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));
    }
}
