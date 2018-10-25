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
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton"/>
    /// </summary>
    public class InkToolbarPenButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarPenButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarPenButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarPenButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton"/>
        /// </summary>
        public InkToolbarPenButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarPenButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarPenButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton"/>.
        /// </summary>
        protected InkToolbarPenButton(string typeName)
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
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.WidthProperty);

            // InkToolbarPenButton specific properties
            Bind(nameof(SelectedStrokeWidth), SelectedStrokeWidthProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedStrokeWidthProperty);
            Bind(nameof(SelectedBrushIndex), SelectedBrushIndexProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushIndexProperty);
            Bind(nameof(Palette), PaletteProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.PaletteProperty);
            Bind(nameof(MinStrokeWidth), MinStrokeWidthProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.MinStrokeWidthProperty);
            Bind(nameof(MaxStrokeWidth), MaxStrokeWidthProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxStrokeWidthProperty);
            Bind(nameof(SelectedBrush), SelectedBrushProperty, Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushProperty, new WindowsXamlHostWrapperConverter());

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxStrokeWidthProperty"/>
        /// </summary>
        public static DependencyProperty MaxStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(MaxStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MinStrokeWidthProperty"/>
        /// </summary>
        public static DependencyProperty MinStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(MinStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.PaletteProperty"/>
        /// </summary>
        public static DependencyProperty PaletteProperty { get; } = DependencyProperty.Register(nameof(Palette), typeof(System.Collections.Generic.IList<Brush>), typeof(InkToolbarPenButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushIndexProperty"/>
        /// </summary>
        public static DependencyProperty SelectedBrushIndexProperty { get; } = DependencyProperty.Register(nameof(SelectedBrushIndex), typeof(int), typeof(InkToolbarPenButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushProperty"/>
        /// </summary>
        public static DependencyProperty SelectedBrushProperty { get; } = DependencyProperty.Register(nameof(SelectedBrush), typeof(Brush), typeof(InkToolbarPenButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedStrokeWidthProperty"/>
        /// </summary>
        public static DependencyProperty SelectedStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(SelectedStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedStrokeWidth"/>
        /// </summary>
        public double SelectedStrokeWidth
        {
            get => (double)GetValue(SelectedStrokeWidthProperty);
            set => SetValue(SelectedStrokeWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushIndex"/>
        /// </summary>
        public int SelectedBrushIndex
        {
            get => (int)GetValue(SelectedBrushIndexProperty);
            set => SetValue(SelectedBrushIndexProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.Palette"/>
        /// </summary>
        public System.Collections.Generic.IList<Brush> Palette
        {
            get => (System.Collections.Generic.IList<Brush>)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MinStrokeWidth"/>
        /// </summary>
        public double MinStrokeWidth
        {
            get => (double)GetValue(MinStrokeWidthProperty);
            set => SetValue(MinStrokeWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxStrokeWidth"/>
        /// </summary>
        public double MaxStrokeWidth
        {
            get => (double)GetValue(MaxStrokeWidthProperty);
            set => SetValue(MaxStrokeWidthProperty, value);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrush"/>
        /// </summary>
        public Brush SelectedBrush => (Brush)GetValue(SelectedBrushProperty);
    }
}