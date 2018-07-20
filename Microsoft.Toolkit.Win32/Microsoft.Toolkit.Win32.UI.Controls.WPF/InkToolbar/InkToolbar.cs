// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Toolkit.Win32.UI.Interop;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbar"/>
    /// </summary>
    [ContentProperty(nameof(Children))]
    public class InkToolbar : WindowsXamlHost
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbar UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.InkToolbar;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbar"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbar"/>
        /// </summary>
        public InkToolbar()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbar).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbar"/> class, a
        /// Wpf-enabled wrapper for <see cref="global::Windows.UI.Xaml.Controls.InkToolbar"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public InkToolbar(string typeName)
            : base(typeName)
        {
            Children = new ObservableCollection<DependencyObject>();
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbar.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbar.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbar.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbar.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbar.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbar.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbar.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbar.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbar.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbar.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbar.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbar.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbar.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbar.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbar.WidthProperty);

            // InkToolbar specific properties
            Bind(nameof(TargetInkCanvas), TargetInkCanvasProperty, global::Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvasProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(IsRulerButtonChecked), IsRulerButtonCheckedProperty, global::Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonCheckedProperty);
            Bind(nameof(InitialControls), InitialControlsProperty, global::Windows.UI.Xaml.Controls.InkToolbar.InitialControlsProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(ActiveTool), ActiveToolProperty, global::Windows.UI.Xaml.Controls.InkToolbar.ActiveToolProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(InkDrawingAttributes), InkDrawingAttributesProperty, global::Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(Orientation), OrientationProperty, global::Windows.UI.Xaml.Controls.InkToolbar.OrientationProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(IsStencilButtonChecked), IsStencilButtonCheckedProperty, global::Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedProperty);
            Bind(nameof(ButtonFlyoutPlacement), ButtonFlyoutPlacementProperty, global::Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacementProperty, new WindowsXamlHostWrapperConverter());

            Children.OfType<WindowsXamlHost>().ToList().ForEach(RelocateChildToUwpControl);
            UwpControl.ActiveToolChanged += OnActiveToolChanged;
            UwpControl.EraseAllClicked += OnEraseAllClicked;
            UwpControl.InkDrawingAttributesChanged += OnInkDrawingAttributesChanged;
            UwpControl.IsRulerButtonCheckedChanged += OnIsRulerButtonCheckedChanged;
            UwpControl.IsStencilButtonCheckedChanged += OnIsStencilButtonCheckedChanged;

            base.OnInitialized(e);
        }

        private void RelocateChildToUwpControl(WindowsXamlHost obj)
        {
            VisualTreeHelper.DisconnectChildrenRecursive(obj.DesktopWindowXamlSource.Content);
            obj.DesktopWindowXamlSource.Content = null;
            Children.Remove(obj);
            UwpControl.Children.Add(obj.XamlRoot);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.ActiveToolProperty"/>
        /// </summary>
        public static DependencyProperty ActiveToolProperty { get; } = DependencyProperty.Register(nameof(ActiveTool), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.InitialControlsProperty"/>
        /// </summary>
        public static DependencyProperty InitialControlsProperty { get; } = DependencyProperty.Register(nameof(InitialControls), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarInitialControls), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesProperty"/>
        /// </summary>
        public static DependencyProperty InkDrawingAttributesProperty { get; } = DependencyProperty.Register(nameof(InkDrawingAttributes), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkDrawingAttributes), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonCheckedProperty"/>
        /// </summary>
        public static DependencyProperty IsRulerButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsRulerButtonChecked), typeof(bool), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvasProperty"/>
        /// </summary>
        public static DependencyProperty TargetInkCanvasProperty { get; } = DependencyProperty.Register(nameof(TargetInkCanvas), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkCanvas), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacementProperty"/>
        /// </summary>
        public static DependencyProperty ButtonFlyoutPlacementProperty { get; } = DependencyProperty.Register(nameof(ButtonFlyoutPlacement), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarButtonFlyoutPlacement), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedProperty"/>
        /// </summary>
        public static DependencyProperty IsStencilButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsStencilButtonChecked), typeof(bool), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.OrientationProperty"/>
        /// </summary>
        public static DependencyProperty OrientationProperty { get; } = DependencyProperty.Register(nameof(Orientation), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.Orientation), typeof(InkToolbar));

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.GetToolButton"/>
        /// </summary>
        /// <returns>InkToolbarToolButton</returns>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton GetToolButton(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarTool tool) => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton)UwpControl.GetToolButton((global::Windows.UI.Xaml.Controls.InkToolbarTool)(int)tool).GetWrapper();

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.GetToggleButton"/>
        /// </summary>
        /// <returns>InkToolbarToggleButton</returns>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToggleButton GetToggleButton(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToggle tool) => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToggleButton)UwpControl.GetToggleButton((global::Windows.UI.Xaml.Controls.InkToolbarToggle)(int)tool).GetWrapper();

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.GetMenuButton"/>
        /// </summary>
        /// <returns>InkToolbarMenuButton</returns>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuButton GetMenuButton(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuKind menu) => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuButton)UwpControl.GetMenuButton((global::Windows.UI.Xaml.Controls.InkToolbarMenuKind)(int)menu).GetWrapper();

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvas"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkCanvas TargetInkCanvas
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkCanvas)GetValue(TargetInkCanvasProperty);
            set => SetValue(TargetInkCanvasProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonChecked"/>
        /// </summary>
        public bool IsRulerButtonChecked
        {
            get => (bool)GetValue(IsRulerButtonCheckedProperty);
            set => SetValue(IsRulerButtonCheckedProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.InitialControls"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarInitialControls InitialControls
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarInitialControls)GetValue(InitialControlsProperty);
            set => SetValue(InitialControlsProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.ActiveTool"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton ActiveTool
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton)GetValue(ActiveToolProperty);
            set => SetValue(ActiveToolProperty, value);
        }

        /// <summary>
        /// Gets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributes"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkDrawingAttributes InkDrawingAttributes
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkDrawingAttributes)GetValue(InkDrawingAttributesProperty);
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.Orientation"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.Orientation Orientation
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonChecked"/>
        /// </summary>
        public bool IsStencilButtonChecked
        {
            get => (bool)GetValue(IsStencilButtonCheckedProperty);
            set => SetValue(IsStencilButtonCheckedProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacement"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarButtonFlyoutPlacement ButtonFlyoutPlacement
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarButtonFlyoutPlacement)GetValue(ButtonFlyoutPlacementProperty);
            set => SetValue(ButtonFlyoutPlacementProperty, value);
        }

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.ActiveToolChanged"/>
        /// </summary>
        public event EventHandler<object> ActiveToolChanged = (sender, args) => { };

        private void OnActiveToolChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.ActiveToolChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.EraseAllClicked"/>
        /// </summary>
        public event EventHandler<object> EraseAllClicked = (sender, args) => { };

        private void OnEraseAllClicked(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.EraseAllClicked?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesChanged"/>
        /// </summary>
        public event EventHandler<object> InkDrawingAttributesChanged = (sender, args) => { };

        private void OnInkDrawingAttributesChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.InkDrawingAttributesChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonCheckedChanged"/>
        /// </summary>
        public event EventHandler<object> IsRulerButtonCheckedChanged = (sender, args) => { };

        private void OnIsRulerButtonCheckedChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.IsRulerButtonCheckedChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="global::Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedChanged"/>
        /// </summary>
        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarIsStencilButtonCheckedChangedEventArgs> IsStencilButtonCheckedChanged = (sender, args) => { };

        private void OnIsStencilButtonCheckedChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            this.IsStencilButtonCheckedChanged?.Invoke(this, args);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<DependencyObject> Children
        {
            get; set;
        }
    }
}