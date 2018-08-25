// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>
    /// </summary>
    public class InkToolbarStencilButton : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkToolbarStencilButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarStencilButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarStencilButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>
        /// </summary>
        public InkToolbarStencilButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarStencilButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarStencilButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>.
        /// </summary>
        protected InkToolbarStencilButton(string typeName)
            : base(typeName)
        {
        }

        /// <inheritdoc />
        protected override void SetContent(Windows.UI.Xaml.UIElement newValue)
        {
            // intentionally empty
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.WidthProperty);

            // InkToolbarStencilButton specific properties
            Bind(nameof(SelectedStencil), SelectedStencilProperty, Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencilProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(IsRulerItemVisible), IsRulerItemVisibleProperty, Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisibleProperty);
            Bind(nameof(IsProtractorItemVisible), IsProtractorItemVisibleProperty, Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisibleProperty);
            Bind(nameof(Protractor), ProtractorProperty, Windows.UI.Xaml.Controls.InkToolbarStencilButton.ProtractorProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(Ruler), RulerProperty, Windows.UI.Xaml.Controls.InkToolbarStencilButton.RulerProperty, new WindowsXamlHostWrapperConverter());

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisibleProperty"/>
        /// </summary>
        public static DependencyProperty IsProtractorItemVisibleProperty { get; } = DependencyProperty.Register(nameof(IsProtractorItemVisible), typeof(bool), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisibleProperty"/>
        /// </summary>
        public static DependencyProperty IsRulerItemVisibleProperty { get; } = DependencyProperty.Register(nameof(IsRulerItemVisible), typeof(bool), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.ProtractorProperty"/>
        /// </summary>
        public static DependencyProperty ProtractorProperty { get; } = DependencyProperty.Register(nameof(Protractor), typeof(InkPresenterProtractor), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.RulerProperty"/>
        /// </summary>
        public static DependencyProperty RulerProperty { get; } = DependencyProperty.Register(nameof(Ruler), typeof(InkPresenterRuler), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencilProperty"/>
        /// </summary>
        public static DependencyProperty SelectedStencilProperty { get; } = DependencyProperty.Register(nameof(SelectedStencil), typeof(InkToolbarStencilKind), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencil"/>
        /// </summary>
        public InkToolbarStencilKind SelectedStencil
        {
            get => (InkToolbarStencilKind)GetValue(SelectedStencilProperty);
            set => SetValue(SelectedStencilProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisible"/>
        /// </summary>
        public bool IsRulerItemVisible
        {
            get => (bool)GetValue(IsRulerItemVisibleProperty);
            set => SetValue(IsRulerItemVisibleProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisible"/>
        /// </summary>
        public bool IsProtractorItemVisible
        {
            get => (bool)GetValue(IsProtractorItemVisibleProperty);
            set => SetValue(IsProtractorItemVisibleProperty, value);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.Protractor"/>
        /// </summary>
        public InkPresenterProtractor Protractor
        {
            get => (InkPresenterProtractor)GetValue(ProtractorProperty);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.Ruler"/>
        /// </summary>
        public InkPresenterRuler Ruler => (InkPresenterRuler)GetValue(RulerProperty);
    }
}