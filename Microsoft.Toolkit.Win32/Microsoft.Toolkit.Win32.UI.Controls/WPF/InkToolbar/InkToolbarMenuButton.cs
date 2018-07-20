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
    /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>
    /// </summary>
    public class InkToolbarMenuButton : WindowsXamlHostBaseExt
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbarMenuButton UwpControl => this.XamlRootInternal as global::Windows.UI.Xaml.Controls.InkToolbarMenuButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarMenuButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>
        /// </summary>
        public InkToolbarMenuButton()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbarMenuButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarMenuButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbarMenuButton(string typeName)
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
            Bind(nameof(IsExtensionGlyphShown), IsExtensionGlyphShownProperty, global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.IsExtensionGlyphShownProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.IsExtensionGlyphShownProperty"/>
        /// </summary>
        public static DependencyProperty IsExtensionGlyphShownProperty { get; } = DependencyProperty.Register(nameof(IsExtensionGlyphShown), typeof(bool), typeof(InkToolbarMenuButton));

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown
        {
            get => (bool)GetValue(IsExtensionGlyphShownProperty);
            set => SetValue(IsExtensionGlyphShownProperty, value);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarMenuButton.MenuKind"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuKind MenuKind
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuKind)(int)UwpControl.MenuKind;
        }
    }
}