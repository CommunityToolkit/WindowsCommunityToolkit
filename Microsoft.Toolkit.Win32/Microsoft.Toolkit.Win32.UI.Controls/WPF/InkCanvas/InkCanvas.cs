using System;
using Microsoft.Windows.Interop;
using Windows.UI.Input.Inking;
using uwpControls = global::Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkCanvas : WindowsXamlHost
    {
        protected uwpControls.InkCanvas UwpControl => this.XamlRoot as uwpControls.InkCanvas;

        // Summary:
        //     Initializes a new instance of the InkCanvas class.
        public InkCanvas()
            : base()
        {
            TypeName = "Windows.UI.Xaml.Controls.InkCanvas";
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, uwpControls.InkCanvas.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, uwpControls.InkCanvas.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, uwpControls.InkCanvas.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, uwpControls.InkCanvas.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, uwpControls.InkCanvas.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, uwpControls.InkCanvas.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, uwpControls.InkCanvas.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, uwpControls.InkCanvas.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, uwpControls.InkCanvas.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, uwpControls.InkCanvas.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, uwpControls.InkCanvas.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, uwpControls.InkCanvas.NameProperty);
            Bind(nameof(Tag), TagProperty, uwpControls.InkCanvas.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, uwpControls.InkCanvas.DataContextProperty);
            Bind(nameof(Width), WidthProperty, uwpControls.InkCanvas.WidthProperty);

            // InkCanvas specific properties
        }

        /// <summary>
        /// Gets the composed ink surface presentation layer
        /// </summary>
        public InkPresenter InkPresenter => UwpControl.InkPresenter;
    }
}