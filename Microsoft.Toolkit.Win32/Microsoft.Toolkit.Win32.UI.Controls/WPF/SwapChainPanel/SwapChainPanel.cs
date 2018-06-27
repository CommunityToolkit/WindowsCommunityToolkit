using System;
using System.Windows;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class SwapChainPanel : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.SwapChainPanel UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.SwapChainPanel;

        public SwapChainPanel()
            : this("Windows.UI.Xaml.Controls.SwapChainPanel")
        {
        }

        // Summary:
        //     Initializes a new instance of the SwapChainPanel class.
        public SwapChainPanel(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.WidthProperty);

            // SwapChainPanel specific properties
            Bind(nameof(CompositionScaleX), CompositionScaleXProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleXProperty);
            Bind(nameof(CompositionScaleY), CompositionScaleYProperty, global::Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleYProperty);
            UwpControl.CompositionScaleChanged += OnCompositionScaleChanged;

            base.OnInitialized(e);
        }

        public static DependencyProperty CompositionScaleXProperty { get; } = DependencyProperty.Register(nameof(CompositionScaleX), typeof(float), typeof(SwapChainPanel));

        public static DependencyProperty CompositionScaleYProperty { get; } = DependencyProperty.Register(nameof(CompositionScaleY), typeof(float), typeof(SwapChainPanel));

        public global::Windows.UI.Core.CoreIndependentInputSource CreateCoreIndependentInputSource(global::Windows.UI.Core.CoreInputDeviceTypes deviceTypes) => UwpControl.CreateCoreIndependentInputSource(deviceTypes);

        public float CompositionScaleX
        {
            get => (float)GetValue(CompositionScaleXProperty);
        }

        public float CompositionScaleY
        {
            get => (float)GetValue(CompositionScaleYProperty);
        }

        public event EventHandler<DynamicForwardedEventArgs> CompositionScaleChanged;

        private void OnCompositionScaleChanged(global::Windows.UI.Xaml.Controls.SwapChainPanel sender, object args)
        {
            this.CompositionScaleChanged?.Invoke(this, new DynamicForwardedEventArgs(args));
        }
    }
}