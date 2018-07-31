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
using uwpControls = Windows.UI.Xaml.Controls;
using uwpInking = Windows.UI.Input.Inking;
using uwpXaml = Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToggleButton"/>
    /// </summary>
    public class InkToolbarToggleButton : WindowsXamlHostBaseExt
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbarToggleButton UwpControl => this.XamlRootInternal as global::Windows.UI.Xaml.Controls.InkToolbarToggleButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToggleButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToggleButton"/>
        /// </summary>
        public InkToolbarToggleButton()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbarToggleButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToggleButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToggleButton"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbarToggleButton(string typeName)
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
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.WidthProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarToggleButton.ToggleKind"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToggle ToggleKind
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToggle)(int)UwpControl.ToggleKind;
        }
    }
}