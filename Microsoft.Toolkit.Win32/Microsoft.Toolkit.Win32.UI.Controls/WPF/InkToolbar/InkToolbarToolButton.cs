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
    /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToolButton"/>
    /// </summary>
    public class InkToolbarToolButton : WindowsXamlHostBaseExt
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbarToolButton UwpControl => this.XamlRootInternal as global::Windows.UI.Xaml.Controls.InkToolbarToolButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToolButton"/>
        /// </summary>
        public InkToolbarToolButton()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbarToolButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToolButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToolButton"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbarToolButton(string typeName)
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
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.WidthProperty);

            // InkToolbarToolButton specific properties
            Bind(nameof(IsExtensionGlyphShown), IsExtensionGlyphShownProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShownProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShownProperty"/>
        /// </summary>
        public static DependencyProperty IsExtensionGlyphShownProperty { get; } = DependencyProperty.Register(nameof(IsExtensionGlyphShown), typeof(bool), typeof(InkToolbarToolButton));

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown
        {
            get => (bool)GetValue(IsExtensionGlyphShownProperty);
            set => SetValue(IsExtensionGlyphShownProperty, value);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToolButton.ToolKind"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarTool ToolKind
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarTool)(int)UwpControl.ToolKind;
        }
    }
}