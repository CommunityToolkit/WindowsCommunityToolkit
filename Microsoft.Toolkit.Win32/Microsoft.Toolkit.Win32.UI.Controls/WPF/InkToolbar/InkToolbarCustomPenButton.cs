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
using Microsoft.Windows.Interop;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using uwpControls = global::Windows.UI.Xaml.Controls;
using uwpInking = Windows.UI.Input.Inking;
using uwpXaml = global::Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>
    /// </summary>
    public class InkToolbarCustomPenButton : WindowsXamlHostBaseExt
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton UwpControl => this.XamlRootInternal as global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomPenButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>
        /// </summary>
        public InkToolbarCustomPenButton()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomPenButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbarCustomPenButton(string typeName)
            : base(typeName)
        {
        }

        protected override void SetHost()
        {
            if (this.VisualParent is InkToolbar parent)
            {
                if (parent.XamlRootInternal is global::Windows.UI.Xaml.Controls.InkToolbar toolbar)
                {
                    toolbar.Children.Add(this.UwpControl);
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.WidthProperty);

            // InkToolbarCustomPenButton specific properties
            Bind(nameof(CustomPen), CustomPenProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPenProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(ConfigurationContent), ConfigurationContentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.ConfigurationContentProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.ConfigurationContentProperty"/>
        /// </summary>
        public static DependencyProperty ConfigurationContentProperty { get; } = DependencyProperty.Register(nameof(ConfigurationContent), typeof(global::Windows.UI.Xaml.UIElement), typeof(InkToolbarCustomPenButton));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPenProperty"/>
        /// </summary>
        public static DependencyProperty CustomPenProperty { get; } = DependencyProperty.Register(nameof(CustomPen), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarCustomPen), typeof(InkToolbarCustomPenButton));

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPen"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarCustomPen CustomPen
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarCustomPen)GetValue(CustomPenProperty);
            set => SetValue(CustomPenProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.ConfigurationContent"/>
        /// </summary>
        public global::Windows.UI.Xaml.UIElement ConfigurationContent
        {
            get => (global::Windows.UI.Xaml.UIElement)GetValue(ConfigurationContentProperty);
            set => SetValue(ConfigurationContentProperty, value);
        }
    }
}