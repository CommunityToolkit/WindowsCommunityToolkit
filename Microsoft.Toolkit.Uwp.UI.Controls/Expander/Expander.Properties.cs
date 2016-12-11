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

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="Expander"/> control allows user to show/hide content based on a boolean state
    /// </summary>
    public partial class Expander : Control
    {
        public static readonly DependencyProperty ExpanderToggleButtonStyleProperty =
            DependencyProperty.Register("ExpanderToggleButtonStyle", typeof(Style), typeof(Expander), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderButtonStyleProperty =
            DependencyProperty.Register("HeaderButtonStyle", typeof(Style), typeof(Expander), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderButtonContentProperty =
            DependencyProperty.Register("HeaderButtonContent", typeof(object), typeof(Expander), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderButtonCommandProperty =
            DependencyProperty.Register("HeaderButtonCommand", typeof(ICommand), typeof(Expander), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(Expander), new PropertyMetadata(null));

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(Expander), new PropertyMetadata(false, OnIsExpandedPropertyChanged));

        public Style ExpanderToggleButtonStyle
        {
            get { return (Style)GetValue(ExpanderToggleButtonStyleProperty); }
            set { SetValue(ExpanderToggleButtonStyleProperty, value); }
        }

        public Style HeaderButtonStyle
        {
            get { return (Style)GetValue(HeaderButtonStyleProperty); }
            set { SetValue(HeaderButtonStyleProperty, value); }
        }

        public object HeaderButtonContent
        {
            get { return GetValue(HeaderButtonContentProperty); }
            set { SetValue(HeaderButtonContentProperty, value); }
        }

        public ICommand HeaderButtonCommand
        {
            get { return (ICommand)GetValue(HeaderButtonCommandProperty); }
            set { SetValue(HeaderButtonCommandProperty, value); }
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        private static void OnIsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var expander = d as Expander;
            if (expander == null || expander._expanderButton == null)
            {
                return;
            }

            var isExpanded = (bool)e.NewValue;
            expander._expanderButton.IsChecked = isExpanded;
            if (isExpanded)
            {
                expander.ExpandControl();
            }
            else
            {
                expander.CollapseControl();
            }
        }
    }
}