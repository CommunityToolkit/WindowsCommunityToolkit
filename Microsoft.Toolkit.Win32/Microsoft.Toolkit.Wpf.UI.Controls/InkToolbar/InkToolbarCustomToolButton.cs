// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>
    /// </summary>
    public class InkToolbarCustomToolButton : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkToolbarCustomToolButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarCustomToolButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>
        /// </summary>
        public InkToolbarCustomToolButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarCustomToolButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>.
        /// </summary>
        protected InkToolbarCustomToolButton(string typeName)
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
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.WidthProperty);

            // InkToolbarCustomToolButton specific properties
            /* Bind(nameof(ConfigurationContent), ConfigurationContentProperty, Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContentProperty); */

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContentProperty"/>
        /// </summary>
        public static DependencyProperty ConfigurationContentProperty { get; } = DependencyProperty.Register(nameof(ConfigurationContent), typeof(UIElement), typeof(InkToolbarCustomToolButton));

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContent"/>
        /// </summary>
        public UIElement ConfigurationContent
        {
            get => (UIElement)GetValue(ConfigurationContentProperty);
            set => SetValue(ConfigurationContentProperty, value);
        }
    }
}