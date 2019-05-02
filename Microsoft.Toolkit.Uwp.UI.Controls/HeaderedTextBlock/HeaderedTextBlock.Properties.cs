// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// Defines the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate),
            typeof(DataTemplate),
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
        /// Gets or sets the header style.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(HeaderTemplateProperty);
            }

            set
            {
                SetValue(HeaderTemplateProperty, value);
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
        /// Gets the collection of inline text elements within a Windows.UI.Xaml.Controls.TextBlock.
        /// </summary>
        /// <returns>
        /// A collection that holds all inline text elements from the Windows.UI.Xaml.Controls.TextBlock. The default is an empty collection.</returns>
        public InlineCollectionWrapper Inlines { get; } = new InlineCollectionWrapper();

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