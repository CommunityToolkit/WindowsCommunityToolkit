// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>
    /// </summary>
    public class InkToolbarMenuButton : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkToolbarMenuButton UwpControl => this.XamlRootInternal as Windows.UI.Xaml.Controls.InkToolbarMenuButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarMenuButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>
        /// </summary>
        public InkToolbarMenuButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarMenuButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarMenuButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbarMenuButton(string typeName)
            : base(typeName)
        {
        }

        protected override void SetContent()
        {
            // intentionally empty
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.WidthProperty);

            // InkToolbarMenuButton specific properties
            Bind(nameof(IsExtensionGlyphShown), IsExtensionGlyphShownProperty, Windows.UI.Xaml.Controls.InkToolbarMenuButton.IsExtensionGlyphShownProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton.IsExtensionGlyphShownProperty"/>
        /// </summary>
        public static DependencyProperty IsExtensionGlyphShownProperty { get; } = DependencyProperty.Register(nameof(IsExtensionGlyphShown), typeof(bool), typeof(InkToolbarMenuButton));

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown
        {
            get => (bool)GetValue(IsExtensionGlyphShownProperty);
            set => SetValue(IsExtensionGlyphShownProperty, value);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton.MenuKind"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuKind MenuKind
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuKind)(int)UwpControl.MenuKind;
        }
    }
}