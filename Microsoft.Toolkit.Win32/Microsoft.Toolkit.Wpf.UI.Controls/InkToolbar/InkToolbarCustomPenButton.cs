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
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>
    /// </summary>
    public class InkToolbarCustomPenButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarCustomPenButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarCustomPenButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomPenButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>
        /// </summary>
        public InkToolbarCustomPenButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarCustomPenButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomPenButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>.
        /// </summary>
        protected InkToolbarCustomPenButton(string typeName)
            : base(typeName)
        {
        }

        /// <inheritdoc />
        protected override void SetContent()
        {
            if (VisualParent is InkToolbar parent)
            {
                if (parent.GetUwpInternalObject() is Windows.UI.Xaml.Controls.InkToolbar toolbar)
                {
                    toolbar.Children.Add(UwpControl);
                }
            }
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.WidthProperty);

            // InkToolbarCustomPenButton specific properties
            Bind(nameof(CustomPen), CustomPenProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPenProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(ConfigurationContent), ConfigurationContentProperty, Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.ConfigurationContentProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.ConfigurationContentProperty"/>
        /// </summary>
        public static DependencyProperty ConfigurationContentProperty { get; } = DependencyProperty.Register(nameof(ConfigurationContent), typeof(Windows.UI.Xaml.UIElement), typeof(InkToolbarCustomPenButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPenProperty"/>
        /// </summary>
        public static DependencyProperty CustomPenProperty { get; } = DependencyProperty.Register(nameof(CustomPen), typeof(InkToolbarCustomPen), typeof(InkToolbarCustomPenButton));

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPen"/>
        /// </summary>
        public InkToolbarCustomPen CustomPen
        {
            get => (InkToolbarCustomPen)GetValue(CustomPenProperty);
            set => SetValue(CustomPenProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.ConfigurationContent"/>
        /// </summary>
        public Windows.UI.Xaml.UIElement ConfigurationContent
        {
            get => (Windows.UI.Xaml.UIElement)GetValue(ConfigurationContentProperty);
            set => SetValue(ConfigurationContentProperty, value);
        }
    }
}