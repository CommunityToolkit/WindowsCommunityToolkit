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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    /// <summary>
    /// Specifies a DefaultButton, modifies a Button Instance
    /// </summary>
    public class DefaultButton : DependencyObject
    {
<<<<<<< HEAD
        /// <summary>
        /// Identifies the <see cref="IsVisible"/> dependency property.
        /// </summary>
=======
        // Using a DependencyProperty as the backing store for IsVisible.  This enables animation, styling, binding, etc...
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(DefaultButton), new PropertyMetadata(true));

        /// <summary>
        /// Specifies the Type of DefaultButton in order to remove it.
        /// </summary>
        /// <param name="type">Type of Default Button</param>
        /// <returns>Removal Object</returns>
        public static DefaultButton OfType(ButtonType type)
        {
            return new DefaultButton { Type = type };
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public override bool Equals(object obj)
        {
            var other = obj as DefaultButton;
            return other != null && other.Type == Type;
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

<<<<<<< HEAD
        /// <inheritdoc/>
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        public override string ToString()
        {
            return Type.ToString();
        }

        private static void IsVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
<<<<<<< HEAD
            if (obj is DefaultButton button && button.Button != null)
=======
            var button = obj as DefaultButton;
            if (button != null && button.Button != null)
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
            {
                var model = button.Button as FrameworkElement;
                model.Visibility = button.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Toolbar Item is Visible
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of Default Button to remove.
        /// </summary>
        public ButtonType Type { get; set; }

        /// <summary>
        /// Gets or sets the instance of button that is removed, in order to preserve any modifications when re-attaching to the Toolbar.
        /// </summary>
        internal IToolbarItem Button
        {
            get
            {
                return _button;
            }

            set
            {
                _button = value;
                if (_button != null)
                {
                    var element = _button as FrameworkElement;
                    element.Visibility = IsVisible ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private IToolbarItem _button;
    }
}