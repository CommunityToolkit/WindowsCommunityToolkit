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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed class ToolbarButton : AppBarButton, IToolbarItem
    {
        public ToolbarButton()
        {
            this.DefaultStyleKey = typeof(ToolbarButton);
        }

        public string ToolTip
        {
            get { return (string)GetValue(ToolTipProperty); }
            set { SetValue(ToolTipProperty, value); }
        }

        public int Position { get; set; } = -1;

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToolTipProperty =
            DependencyProperty.Register(nameof(ToolTip), typeof(string), typeof(ToolbarButton), new PropertyMetadata(null, ToolTipChanged));

        public static void ToolTipChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ToolbarButton button)
            {
                ToolTipService.SetToolTip(button, button.ToolTip);
            }
        }
    }
}