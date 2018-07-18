// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkToolbarStencilButton : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.InkToolbarStencilButton UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.InkToolbarStencilButton;

        public InkToolbarStencilButton()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbarStencilButton).FullName)
        {
        }

        // Summary:
        //     Initializes a new instance of the InkToolbarStencilButton class.
        public InkToolbarStencilButton(string typeName)
            : base(typeName)
        {
        }

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
            Bind(nameof(SelectedStencil), SelectedStencilProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencilProperty);
            Bind(nameof(IsRulerItemVisible), IsRulerItemVisibleProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisibleProperty);
            Bind(nameof(IsProtractorItemVisible), IsProtractorItemVisibleProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisibleProperty);
            Bind(nameof(Protractor), ProtractorProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.ProtractorProperty);
            Bind(nameof(Ruler), RulerProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.RulerProperty);

            base.OnInitialized(e);
        }

        public static DependencyProperty IsProtractorItemVisibleProperty { get; } = DependencyProperty.Register(nameof(IsProtractorItemVisible), typeof(bool), typeof(InkToolbarStencilButton));

        public static DependencyProperty IsRulerItemVisibleProperty { get; } = DependencyProperty.Register(nameof(IsRulerItemVisible), typeof(bool), typeof(InkToolbarStencilButton));

        public static DependencyProperty ProtractorProperty { get; } = DependencyProperty.Register(nameof(Protractor), typeof(global::Windows.UI.Input.Inking.InkPresenterProtractor), typeof(InkToolbarStencilButton));

        public static DependencyProperty RulerProperty { get; } = DependencyProperty.Register(nameof(Ruler), typeof(global::Windows.UI.Input.Inking.InkPresenterRuler), typeof(InkToolbarStencilButton));

        public static DependencyProperty SelectedStencilProperty { get; } = DependencyProperty.Register(nameof(SelectedStencil), typeof(global::Windows.UI.Xaml.Controls.InkToolbarStencilKind), typeof(InkToolbarStencilButton));

        public global::Windows.UI.Xaml.Controls.InkToolbarStencilKind SelectedStencil
        {
            get => (global::Windows.UI.Xaml.Controls.InkToolbarStencilKind)GetValue(SelectedStencilProperty);
            set => SetValue(SelectedStencilProperty, value);
        }

        public bool IsRulerItemVisible
        {
            get => (bool)GetValue(IsRulerItemVisibleProperty);
            set => SetValue(IsRulerItemVisibleProperty, value);
        }

        public bool IsProtractorItemVisible
        {
            get => (bool)GetValue(IsProtractorItemVisibleProperty);
            set => SetValue(IsProtractorItemVisibleProperty, value);
        }

        public global::Windows.UI.Input.Inking.InkPresenterProtractor Protractor
        {
            get => (global::Windows.UI.Input.Inking.InkPresenterProtractor)GetValue(ProtractorProperty);
        }

        public global::Windows.UI.Input.Inking.InkPresenterRuler Ruler
        {
            get => (global::Windows.UI.Input.Inking.InkPresenterRuler)GetValue(RulerProperty);
        }
    }
}