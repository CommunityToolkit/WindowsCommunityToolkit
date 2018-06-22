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
    [ContentProperty(nameof(Children))]
    public class InkToolbar : WindowsXamlHost
    {
        protected uwpControls.InkToolbar UwpControl => this.XamlRoot as uwpControls.InkToolbar;

        public InkToolbar()
            : this("Windows.UI.Xaml.Controls.InkToolbar")
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
            Bind(nameof(Style), StyleProperty, uwpControls.InkToolbar.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, uwpControls.InkToolbar.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, uwpControls.InkToolbar.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, uwpControls.InkToolbar.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, uwpControls.InkToolbar.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, uwpControls.InkToolbar.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, uwpControls.InkToolbar.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, uwpControls.InkToolbar.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, uwpControls.InkToolbar.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, uwpControls.InkToolbar.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, uwpControls.InkToolbar.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, uwpControls.InkToolbar.NameProperty);
            Bind(nameof(Tag), TagProperty, uwpControls.InkToolbar.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, uwpControls.InkToolbar.DataContextProperty);
            Bind(nameof(Width), WidthProperty, uwpControls.InkToolbar.WidthProperty);

            // InkToolbar specific properties
            Bind(nameof(ActiveTool), ActiveToolProperty, uwpControls.InkToolbar.ActiveToolProperty);
            Bind(nameof(InkDrawingAttributes), InkDrawingAttributesProperty, uwpControls.InkToolbar.InkDrawingAttributesProperty);
            Bind(nameof(Orientation), OrientationProperty, uwpControls.InkToolbar.OrientationProperty);
            Bind(nameof(IsStencilButtonChecked), IsStencilButtonCheckedProperty, uwpControls.InkToolbar.IsStencilButtonCheckedProperty);
            Bind(nameof(ButtonFlyoutPlacement), ButtonFlyoutPlacementProperty, uwpControls.InkToolbar.ButtonFlyoutPlacementProperty);
            Bind(nameof(InitialControls), InitialControlsProperty, uwpControls.InkToolbar.InitialControlsProperty);
            Bind(nameof(IsRulerButtonChecked), IsRulerButtonCheckedProperty, uwpControls.InkToolbar.IsRulerButtonCheckedProperty);
            Bind(nameof(TargetInkCanvas), TargetInkCanvasProperty, uwpControls.InkToolbar.TargetInkCanvasProperty, new WindowsXamlHostWrapperConverter());

            Children.OfType<WindowsXamlHost>().ToList().ForEach(RelocateChildToUwpControl);

            base.OnInitialized(e);
        }

        private void RelocateChildToUwpControl(WindowsXamlHost obj)
        {
            VisualTreeHelper.DisconnectChildrenRecursive(obj.desktopWindowXamlSource.Content);
            obj.desktopWindowXamlSource.Content = null;
            Children.Remove(obj);
            UwpControl.Children.Add(obj.XamlRoot);
        }

        public static DependencyProperty ActiveToolProperty { get; } = DependencyProperty.Register(nameof(ActiveTool), typeof(uwpControls.InkToolbarToolButton), typeof(InkToolbar));

        public static DependencyProperty InitialControlsProperty { get; } = DependencyProperty.Register(nameof(InitialControls), typeof(uwpControls.InkToolbarInitialControls), typeof(InkToolbar));

        public static DependencyProperty InkDrawingAttributesProperty { get; } = DependencyProperty.Register(nameof(InkDrawingAttributes), typeof(uwpInking.InkDrawingAttributes), typeof(InkToolbar));

        public static DependencyProperty IsRulerButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsRulerButtonChecked), typeof(bool), typeof(InkToolbar));

        public static DependencyProperty TargetInkCanvasProperty { get; } = DependencyProperty.Register(nameof(TargetInkCanvas), typeof(InkCanvas), typeof(InkToolbar));

        public static DependencyProperty ButtonFlyoutPlacementProperty { get; } = DependencyProperty.Register(nameof(ButtonFlyoutPlacement), typeof(uwpControls.InkToolbarButtonFlyoutPlacement), typeof(InkToolbar));

        public static DependencyProperty IsStencilButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsStencilButtonChecked), typeof(bool), typeof(InkToolbar));

        public static DependencyProperty OrientationProperty { get; } = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(InkToolbar));

        public uwpControls.InkToolbarToolButton GetToolButton(uwpControls.InkToolbarTool tool) => UwpControl.GetToolButton(tool);

        public uwpControls.InkToolbarToggleButton GetToggleButton(uwpControls.InkToolbarToggle tool) => UwpControl.GetToggleButton(tool);

        public uwpControls.InkToolbarMenuButton GetMenuButton(uwpControls.InkToolbarMenuKind menu) => UwpControl.GetMenuButton(menu);

        public InkCanvas TargetInkCanvas
        {
            get => (InkCanvas)GetValue(TargetInkCanvasProperty); set => SetValue(TargetInkCanvasProperty, value);
        }

        public bool IsRulerButtonChecked
        {
            get => (bool)GetValue(IsRulerButtonCheckedProperty); set => SetValue(IsRulerButtonCheckedProperty, value);
        }

        public uwpControls.InkToolbarInitialControls InitialControls
        {
            get => (uwpControls.InkToolbarInitialControls)GetValue(InitialControlsProperty); set => SetValue(InitialControlsProperty, value);
        }

        public uwpControls.InkToolbarToolButton ActiveTool
        {
            get => (uwpControls.InkToolbarToolButton)GetValue(ActiveToolProperty); set => SetValue(ActiveToolProperty, value);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<DependencyObject> Children
        {
            get; set;
        }

        public uwpInking.InkDrawingAttributes InkDrawingAttributes { get => (uwpInking.InkDrawingAttributes)GetValue(InkDrawingAttributesProperty); }

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value);
        }

        public bool IsStencilButtonChecked
        {
            get => (bool)GetValue(IsStencilButtonCheckedProperty); set => SetValue(IsStencilButtonCheckedProperty, value);
        }

        public uwpControls.InkToolbarButtonFlyoutPlacement ButtonFlyoutPlacement
        {
            get => (uwpControls.InkToolbarButtonFlyoutPlacement)GetValue(ButtonFlyoutPlacementProperty); set => SetValue(ButtonFlyoutPlacementProperty, value);
        }

        public event TypedEventHandler<uwpControls.InkToolbar, object> ActiveToolChanged
        {
            add { UwpControl.ActiveToolChanged += value; } remove { UwpControl.ActiveToolChanged -= value; }
        }

        public event TypedEventHandler<uwpControls.InkToolbar, object> EraseAllClicked
        {
            add { UwpControl.EraseAllClicked += value; } remove { UwpControl.EraseAllClicked -= value; }
        }

        public event TypedEventHandler<uwpControls.InkToolbar, object> InkDrawingAttributesChanged
        {
            add { UwpControl.InkDrawingAttributesChanged += value; } remove { UwpControl.InkDrawingAttributesChanged -= value; }
        }

        public event TypedEventHandler<uwpControls.InkToolbar, object> IsRulerButtonCheckedChanged
        {
            add { UwpControl.IsRulerButtonCheckedChanged += value; } remove { UwpControl.IsRulerButtonCheckedChanged -= value; }
        }

        public event TypedEventHandler<uwpControls.InkToolbar, uwpControls.InkToolbarIsStencilButtonCheckedChangedEventArgs> IsStencilButtonCheckedChanged
        {
            add { UwpControl.IsStencilButtonCheckedChanged += value; } remove { UwpControl.IsStencilButtonCheckedChanged -= value; }
        }
    }
}