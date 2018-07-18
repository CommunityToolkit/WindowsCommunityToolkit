// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Toolkit.Win32.UI.Interop;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    [ContentProperty(nameof(Children))]
    public class InkToolbar : WindowsXamlHost
    {
        public global::Windows.UI.Xaml.Controls.InkToolbar UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.InkToolbar;

        public InkToolbar()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbar).FullName)
        {
        }

        // Summary:
        //     Initializes a new instance of the InkToolbar class.
        public InkToolbar(string typeName)
            : base(typeName)
        {
            Children = new List<DependencyObject>();
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
            Bind(nameof(InitialControls), InitialControlsProperty, global::Windows.UI.Xaml.Controls.InkToolbar.InitialControlsProperty);
            Bind(nameof(ActiveTool), ActiveToolProperty, global::Windows.UI.Xaml.Controls.InkToolbar.ActiveToolProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(InkDrawingAttributes), InkDrawingAttributesProperty, global::Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesProperty);
            Bind(nameof(Orientation), OrientationProperty, global::Windows.UI.Xaml.Controls.InkToolbar.OrientationProperty);
            Bind(nameof(IsStencilButtonChecked), IsStencilButtonCheckedProperty, global::Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedProperty);
            Bind(nameof(ButtonFlyoutPlacement), ButtonFlyoutPlacementProperty, global::Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacementProperty);

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
            VisualTreeHelper.DisconnectChildrenRecursive(obj.desktopWindowXamlSource.Content);
            obj.desktopWindowXamlSource.Content = null;
            Children.Remove(obj);
            UwpControl.Children.Add(obj.XamlRoot);
        }

        public static DependencyProperty ActiveToolProperty { get; } = DependencyProperty.Register(nameof(ActiveTool), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton), typeof(InkToolbar));

        public static DependencyProperty InitialControlsProperty { get; } = DependencyProperty.Register(nameof(InitialControls), typeof(global::Windows.UI.Xaml.Controls.InkToolbarInitialControls), typeof(InkToolbar));

        public static DependencyProperty InkDrawingAttributesProperty { get; } = DependencyProperty.Register(nameof(InkDrawingAttributes), typeof(global::Windows.UI.Input.Inking.InkDrawingAttributes), typeof(InkToolbar));

        public static DependencyProperty IsRulerButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsRulerButtonChecked), typeof(bool), typeof(InkToolbar));

        public static DependencyProperty TargetInkCanvasProperty { get; } = DependencyProperty.Register(nameof(TargetInkCanvas), typeof(Microsoft.Toolkit.Win32.UI.Controls.WPF.InkCanvas), typeof(InkToolbar));

        public static DependencyProperty ButtonFlyoutPlacementProperty { get; } = DependencyProperty.Register(nameof(ButtonFlyoutPlacement), typeof(global::Windows.UI.Xaml.Controls.InkToolbarButtonFlyoutPlacement), typeof(InkToolbar));

        public static DependencyProperty IsStencilButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsStencilButtonChecked), typeof(bool), typeof(InkToolbar));

        public static DependencyProperty OrientationProperty { get; } = DependencyProperty.Register(nameof(Orientation), typeof(global::Windows.UI.Xaml.Controls.Orientation), typeof(InkToolbar));

        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton GetToolButton(global::Windows.UI.Xaml.Controls.InkToolbarTool tool) => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton)UwpControl.GetToolButton(tool).GetWrapper();

        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToggleButton GetToggleButton(global::Windows.UI.Xaml.Controls.InkToolbarToggle tool) => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToggleButton)UwpControl.GetToggleButton(tool).GetWrapper();

        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuButton GetMenuButton(global::Windows.UI.Xaml.Controls.InkToolbarMenuKind menu) => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarMenuButton)UwpControl.GetMenuButton(menu).GetWrapper();

        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkCanvas TargetInkCanvas
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkCanvas)GetValue(TargetInkCanvasProperty);
            set => SetValue(TargetInkCanvasProperty, value);
        }

        public bool IsRulerButtonChecked
        {
            get => (bool)GetValue(IsRulerButtonCheckedProperty);
            set => SetValue(IsRulerButtonCheckedProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.InkToolbarInitialControls InitialControls
        {
            get => (global::Windows.UI.Xaml.Controls.InkToolbarInitialControls)GetValue(InitialControlsProperty);
            set => SetValue(InitialControlsProperty, value);
        }

        public Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton ActiveTool
        {
            get => (Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarToolButton)GetValue(ActiveToolProperty);
            set => SetValue(ActiveToolProperty, value);
        }

        public global::Windows.UI.Input.Inking.InkDrawingAttributes InkDrawingAttributes
        {
            get => (global::Windows.UI.Input.Inking.InkDrawingAttributes)GetValue(InkDrawingAttributesProperty);
        }

        public global::Windows.UI.Xaml.Controls.Orientation Orientation
        {
            get => (global::Windows.UI.Xaml.Controls.Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public bool IsStencilButtonChecked
        {
            get => (bool)GetValue(IsStencilButtonCheckedProperty);
            set => SetValue(IsStencilButtonCheckedProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.InkToolbarButtonFlyoutPlacement ButtonFlyoutPlacement
        {
            get => (global::Windows.UI.Xaml.Controls.InkToolbarButtonFlyoutPlacement)GetValue(ButtonFlyoutPlacementProperty);
            set => SetValue(ButtonFlyoutPlacementProperty, value);
        }

        public event EventHandler<object> ActiveToolChanged = (sender, args) => { };

        private void OnActiveToolChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.ActiveToolChanged?.Invoke(this, args);
        }

        public event EventHandler<object> EraseAllClicked = (sender, args) => { };

        private void OnEraseAllClicked(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.EraseAllClicked?.Invoke(this, args);
        }

        public event EventHandler<object> InkDrawingAttributesChanged = (sender, args) => { };

        private void OnInkDrawingAttributesChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.InkDrawingAttributesChanged?.Invoke(this, args);
        }

        public event EventHandler<object> IsRulerButtonCheckedChanged = (sender, args) => { };

        private void OnIsRulerButtonCheckedChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            this.IsRulerButtonCheckedChanged?.Invoke(this, args);
        }

        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WPF.InkToolbarIsStencilButtonCheckedChangedEventArgs> IsStencilButtonCheckedChanged = (sender, args) => { };

        private void OnIsStencilButtonCheckedChanged(global::Windows.UI.Xaml.Controls.InkToolbar sender, global::Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            this.IsStencilButtonCheckedChanged?.Invoke(this, args);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<DependencyObject> Children
        {
            get; set;
        }
    }
}