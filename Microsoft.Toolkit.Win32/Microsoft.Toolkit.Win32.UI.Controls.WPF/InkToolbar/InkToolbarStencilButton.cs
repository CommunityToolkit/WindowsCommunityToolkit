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
    /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>
    /// </summary>
    public class InkToolbarStencilButton : WindowsXamlHostBaseExt
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbarStencilButton UwpControl => this.XamlRootInternal as global::Windows.UI.Xaml.Controls.InkToolbarStencilButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarStencilButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>
        /// </summary>
        public InkToolbarStencilButton()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbarStencilButton).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarStencilButton"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbarStencilButton(string typeName)
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
            Bind(nameof(SelectedStencil), SelectedStencilProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencilProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(IsRulerItemVisible), IsRulerItemVisibleProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisibleProperty);
            Bind(nameof(IsProtractorItemVisible), IsProtractorItemVisibleProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisibleProperty);
            Bind(nameof(Protractor), ProtractorProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.ProtractorProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(Ruler), RulerProperty, global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.RulerProperty, new WindowsXamlHostWrapperConverter());

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisibleProperty"/>
        /// </summary>
        public static DependencyProperty IsProtractorItemVisibleProperty { get; } = DependencyProperty.Register(nameof(IsProtractorItemVisible), typeof(bool), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisibleProperty"/>
        /// </summary>
        public static DependencyProperty IsRulerItemVisibleProperty { get; } = DependencyProperty.Register(nameof(IsRulerItemVisible), typeof(bool), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.ProtractorProperty"/>
        /// </summary>
        public static DependencyProperty ProtractorProperty { get; } = DependencyProperty.Register(nameof(Protractor), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenterProtractor), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.RulerProperty"/>
        /// </summary>
        public static DependencyProperty RulerProperty { get; } = DependencyProperty.Register(nameof(Ruler), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenterRuler), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencilProperty"/>
        /// </summary>
        public static DependencyProperty SelectedStencilProperty { get; } = DependencyProperty.Register(nameof(SelectedStencil), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarStencilKind), typeof(InkToolbarStencilButton));

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencil"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarStencilKind SelectedStencil
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarStencilKind)GetValue(SelectedStencilProperty);
            set => SetValue(SelectedStencilProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisible"/>
        /// </summary>
        public bool IsRulerItemVisible
        {
            get => (bool)GetValue(IsRulerItemVisibleProperty);
            set => SetValue(IsRulerItemVisibleProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisible"/>
        /// </summary>
        public bool IsProtractorItemVisible
        {
            get => (bool)GetValue(IsProtractorItemVisibleProperty);
            set => SetValue(IsProtractorItemVisibleProperty, value);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.Protractor"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenterProtractor Protractor
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenterProtractor)GetValue(ProtractorProperty);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbarStencilButton.Ruler"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenterRuler Ruler
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkPresenterRuler)GetValue(RulerProperty);
        }
    }
}