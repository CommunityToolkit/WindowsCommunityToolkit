using System;
using System.Windows;
using Microsoft.Windows.Interop;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using uwpControls = global::Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class SwapChainPanel : WindowsXamlHost
    {
        protected virtual uwpControls.SwapChainPanel UwpControl => this.XamlRoot as uwpControls.SwapChainPanel;

        // Summary:
        //     Initializes a new instance of the SwapChainPanel class.
        public SwapChainPanel()
            : this("Windows.UI.Xaml.Controls.SwapChainPanel")
        {
        }

        public SwapChainPanel(string typeName)
            : base(typeName)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, uwpControls.SwapChainPanel.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, uwpControls.SwapChainPanel.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, uwpControls.SwapChainPanel.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, uwpControls.SwapChainPanel.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, uwpControls.SwapChainPanel.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, uwpControls.SwapChainPanel.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, uwpControls.SwapChainPanel.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, uwpControls.SwapChainPanel.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, uwpControls.SwapChainPanel.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, uwpControls.SwapChainPanel.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, uwpControls.SwapChainPanel.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, uwpControls.SwapChainPanel.NameProperty);
            Bind(nameof(Tag), TagProperty, uwpControls.SwapChainPanel.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, uwpControls.SwapChainPanel.DataContextProperty);
            Bind(nameof(Width), WidthProperty, uwpControls.SwapChainPanel.WidthProperty);

            // SwapChainPanel specific properties
            Bind(nameof(CompositionScaleX), CompositionScaleXProperty, uwpControls.SwapChainPanel.CompositionScaleXProperty);
            Bind(nameof(CompositionScaleY), CompositionScaleYProperty, uwpControls.SwapChainPanel.CompositionScaleYProperty);
        }

        public static DependencyProperty CompositionScaleXProperty { get; } = DependencyProperty.Register(nameof(CompositionScaleX), typeof(float), typeof(SwapChainPanel));

        public static DependencyProperty CompositionScaleYProperty { get; } = DependencyProperty.Register(nameof(CompositionScaleY), typeof(float), typeof(SwapChainPanel));

        public CoreIndependentInputSource CreateCoreIndependentInputSource(CoreInputDeviceTypes deviceTypes) => UwpControl.CreateCoreIndependentInputSource(deviceTypes);

        public float CompositionScaleX { get => (float)GetValue(CompositionScaleXProperty); }

        public float CompositionScaleY { get => (float)GetValue(CompositionScaleYProperty); }

        public event TypedEventHandler<uwpControls.SwapChainPanel, object> CompositionScaleChanged
        {
            add { UwpControl.CompositionScaleChanged += value; }
            remove { UwpControl.CompositionScaleChanged -= value; }
        }
    }
}