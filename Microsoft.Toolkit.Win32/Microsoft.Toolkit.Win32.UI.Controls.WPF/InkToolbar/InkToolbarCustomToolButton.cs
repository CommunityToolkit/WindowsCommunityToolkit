// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Toolkit.Win32.UI.Interop;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using uwpControls = global::Windows.UI.Xaml.Controls;
using uwpInking = Windows.UI.Input.Inking;
using uwpXaml = global::Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>
    /// </summary>
    public class InkToolbarCustomToolButton : WindowsXamlHostBaseExt
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton UwpControl => this.XamlRootInternal as global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>
        /// </summary>
        public InkToolbarCustomToolButton()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbarCustomToolButton(string typeName)
            : base(typeName)
        {
        }

        protected override void SetHost()
        {
            // intentionally empty
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.WidthProperty);

            // InkToolbarCustomToolButton specific properties
            /* Bind(nameof(ConfigurationContent), ConfigurationContentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContentProperty); */

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContentProperty"/>
        /// </summary>
        public static DependencyProperty ConfigurationContentProperty { get; } = DependencyProperty.Register(nameof(ConfigurationContent), typeof(UIElement), typeof(InkToolbarCustomToolButton));

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContent"/>
        /// </summary>
        public UIElement ConfigurationContent
        {
            get => (UIElement)GetValue(ConfigurationContentProperty);
            set => SetValue(ConfigurationContentProperty, value);
        }
    }
}