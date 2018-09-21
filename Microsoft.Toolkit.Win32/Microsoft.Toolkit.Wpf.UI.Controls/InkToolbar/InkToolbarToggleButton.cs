// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToggleButton"/>
    /// </summary>
    public class InkToolbarToggleButton : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkToolbarToggleButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarToggleButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToggleButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToggleButton"/>
        /// </summary>
        public InkToolbarToggleButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarToggleButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToggleButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToggleButton"/>.
        /// </summary>
        protected InkToolbarToggleButton(string typeName)
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
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.InkToolbarToggleButton.WidthProperty);

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarToggleButton.ToggleKind"/>
        /// </summary>
        public InkToolbarToggle ToggleKind => (InkToolbarToggle)UwpControl.ToggleKind;
    }
}