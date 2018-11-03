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
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.RichEditBox"/>
    /// </summary>
    public class RichEditBox : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.RichEditBox UwpControl => ChildInternal as Windows.UI.Xaml.Controls.RichEditBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichEditBox"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.RichEditBox"/>
        /// </summary>
        public RichEditBox()
            : this(typeof(Windows.UI.Xaml.Controls.RichEditBox).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RichEditBox"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.RichEditBox"/>.
        /// </summary>
        protected RichEditBox(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.RichEditBox.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.RichEditBox.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.RichEditBox.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.RichEditBox.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.RichEditBox.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.RichEditBox.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.RichEditBox.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.RichEditBox.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.RichEditBox.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.RichEditBox.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.RichEditBox.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.RichEditBox.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.RichEditBox.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.RichEditBox.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.RichEditBox.WidthProperty);

            // properties of RichEditBox
            Bind(nameof(PlaceholderText), PlaceholderTextProperty, Windows.UI.Xaml.Controls.RichEditBox.PlaceholderTextProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.RichEditBox.PlaceholderTextProperty"/>
        /// </summary>
        public static DependencyProperty PlaceholderTextProperty { get; } = DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(RichEditBox));

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.RichEditBox.PlaceholderText"/>
        /// </summary>
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }
    }
}
