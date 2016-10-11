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
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines the properties for the <see cref="HeaderedTextBlock"/> control.
    /// </summary>
    public partial class HeaderedTextBlock
    {
        /// <summary>
        /// Defines the <see cref="HeaderFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.Register(
            nameof(HeaderFontSize),
            typeof(double),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(default(double)));

        /// <summary>
        /// Defines the <see cref="TextFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextFontSizeProperty = DependencyProperty.Register(
            nameof(TextFontSize),
            typeof(double),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(default(double)));

        /// <summary>
        /// Defines the <see cref="HeaderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register(
            nameof(HeaderStyle),
            typeof(Style),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="TextStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register(
            nameof(TextStyle),
            typeof(Style),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(null, (d, e) => { ((HeaderedTextBlock)d).UpdateVisibility(); }));

        /// <summary>
        /// Defines the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(null, (d, e) => { ((HeaderedTextBlock)d).UpdateVisibility(); }));

        /// <summary>
        /// Defines the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation),
            typeof(Orientation),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(Orientation.Vertical, (d, e) => { ((HeaderedTextBlock)d).UpdateForOrientation((Orientation)e.NewValue); }));

        /// <summary>
        /// Defines the <see cref="HideTextIfEmpty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HideTextIfEmptyProperty = DependencyProperty.Register(
            nameof(HideTextIfEmpty),
            typeof(bool),
            typeof(HeaderedTextBlock),
            new PropertyMetadata(false, (d, e) => { ((HeaderedTextBlock)d).UpdateVisibility(); }));

        /// <summary>
        /// Gets or sets the HeaderContent font size.
        /// </summary>
        public double HeaderFontSize
        {
            get
            {
                return (double)GetValue(HeaderFontSizeProperty);
            }

            set
            {
                SetValue(HeaderFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the TextContent font size.
        /// </summary>
        public double TextFontSize
        {
            get
            {
                return (double)GetValue(TextFontSizeProperty);
            }

            set
            {
                SetValue(TextFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the header style.
        /// </summary>
        public Style HeaderStyle
        {
            get
            {
                return (Style)GetValue(HeaderStyleProperty);
            }

            set
            {
                SetValue(HeaderStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text style.
        /// </summary>
        public Style TextStyle
        {
            get
            {
                return (Style)GetValue(TextStyleProperty);
            }

            set
            {
                SetValue(TextStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header
        {
            get
            {
                return (string)GetValue(HeaderProperty);
            }

            set
            {
                SetValue(HeaderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return (Orientation)GetValue(OrientationProperty);
            }

            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Text TextBlock is hidden if its value is empty
        /// </summary>
        public bool HideTextIfEmpty
        {
            get
            {
                return (bool)GetValue(HideTextIfEmptyProperty);
            }

            set
            {
                SetValue(HideTextIfEmptyProperty, value);
            }
        }
    }
}