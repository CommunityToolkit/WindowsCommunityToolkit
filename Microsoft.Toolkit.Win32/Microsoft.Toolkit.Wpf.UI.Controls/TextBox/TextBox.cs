// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.XamlHost;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.TextBox"/>
    /// </summary>
    public class TextBox : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.TextBox UwpControl => ChildInternal as Windows.UI.Xaml.Controls.TextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.TextBox"/>
        /// </summary>
        public TextBox()
            : this(typeof(Windows.UI.Xaml.Controls.TextBox).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.TextBox"/>.
        /// </summary>
        protected TextBox(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.TextBox.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.TextBox.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.TextBox.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.TextBox.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.TextBox.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.TextBox.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.TextBox.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.TextBox.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.TextBox.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.TextBox.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.TextBox.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.TextBox.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.TextBox.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.TextBox.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.TextBox.WidthProperty);

            // properties of TextBox
            Bind(nameof(PlaceholderText), PlaceholderTextProperty, Windows.UI.Xaml.Controls.TextBox.PlaceholderTextProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.TextBox.PlaceholderTextProperty"/>
        /// </summary>
        public static DependencyProperty PlaceholderTextProperty { get; } = DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(TextBox));

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.TextBox.PlaceholderText"/>
        /// </summary>
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }
    }
}
