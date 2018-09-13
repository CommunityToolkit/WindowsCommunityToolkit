using System;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.SwapChainPanel"/>
    /// </summary>
    public class SwapChainPanel : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.SwapChainPanel UwpControl => ChildInternal as Windows.UI.Xaml.Controls.SwapChainPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapChainPanel"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.SwapChainPanel"/>
        /// </summary>
        public SwapChainPanel()
            : this(typeof(Windows.UI.Xaml.Controls.SwapChainPanel).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwapChainPanel"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.SwapChainPanel"/>.
        /// Intended for internal framework use only.
        /// </summary>
        public SwapChainPanel(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.SwapChainPanel.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.SwapChainPanel.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.SwapChainPanel.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.SwapChainPanel.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.SwapChainPanel.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.SwapChainPanel.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.SwapChainPanel.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.SwapChainPanel.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.SwapChainPanel.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.SwapChainPanel.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.SwapChainPanel.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.SwapChainPanel.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.SwapChainPanel.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.SwapChainPanel.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.SwapChainPanel.WidthProperty);

            // SwapChainPanel specific properties
            Bind(nameof(CompositionScaleX), CompositionScaleXProperty, Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleXProperty);
            Bind(nameof(CompositionScaleY), CompositionScaleYProperty, Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleYProperty);
            UwpControl.CompositionScaleChanged += OnCompositionScaleChanged;

            base.OnInitialized(e);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleXProperty"/>
        /// </summary>
        public static DependencyProperty CompositionScaleXProperty { get; } = DependencyProperty.Register(nameof(CompositionScaleX), typeof(float), typeof(SwapChainPanel));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleYProperty"/>
        /// </summary>
        public static DependencyProperty CompositionScaleYProperty { get; } = DependencyProperty.Register(nameof(CompositionScaleY), typeof(float), typeof(SwapChainPanel));

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CreateCoreIndependentInputSource"/>
        /// </summary>
        /// <returns>CoreIndependentInputSource</returns>
        public CoreIndependentInputSource CreateCoreIndependentInputSource(CoreInputDeviceTypes deviceTypes) => UwpControl.CreateCoreIndependentInputSource((Windows.UI.Core.CoreInputDeviceTypes)deviceTypes);

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleX"/>
        /// </summary>
        public float CompositionScaleX
        {
            get => (float)GetValue(CompositionScaleXProperty);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleY"/>
        /// </summary>
        public float CompositionScaleY
        {
            get => (float)GetValue(CompositionScaleYProperty);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.SwapChainPanel.CompositionScaleChanged"/>
        /// </summary>
        public event EventHandler<object> CompositionScaleChanged = (sender, args) => { };

        private void OnCompositionScaleChanged(Windows.UI.Xaml.Controls.SwapChainPanel sender, object args)
        {
            this.CompositionScaleChanged?.Invoke(this, args);
        }
    }
}