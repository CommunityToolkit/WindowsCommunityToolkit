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
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton"/>
    /// </summary>
    public class InkToolbarToolButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarToolButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarToolButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton"/>
        /// </summary>
        public InkToolbarToolButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarToolButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton"/>.
        /// </summary>
        protected InkToolbarToolButton(string typeName)
            : base(typeName)
        {
        }

        /// <inheritdoc />
        protected override void SetContent()
        {
            // intentionally empty
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.WidthProperty);

            // InkToolbarToolButton specific properties
            Bind(nameof(IsExtensionGlyphShown), IsExtensionGlyphShownProperty, Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShownProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShownProperty"/>
        /// </summary>
        public static DependencyProperty IsExtensionGlyphShownProperty { get; } = DependencyProperty.Register(nameof(IsExtensionGlyphShown), typeof(bool), typeof(InkToolbarToolButton));

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown
        {
            get => (bool)GetValue(IsExtensionGlyphShownProperty);
            set => SetValue(IsExtensionGlyphShownProperty, value);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.ToolKind"/>
        /// </summary>
        public InkToolbarTool ToolKind => (InkToolbarTool)UwpControl.ToolKind;
    }
}