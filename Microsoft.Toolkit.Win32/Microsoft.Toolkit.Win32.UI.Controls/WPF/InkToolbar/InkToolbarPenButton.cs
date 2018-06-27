using System;
using System.Windows;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkToolbarPenButton : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.InkToolbarPenButton UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.InkToolbarPenButton;

        public InkToolbarPenButton()
            : this("Windows.UI.Xaml.Controls.InkToolbarPenButton")
        {
        }

        // Summary:
        //     Initializes a new instance of the InkToolbarPenButton class.
        public InkToolbarPenButton(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.WidthProperty);

            // InkToolbarPenButton specific properties
            Bind(nameof(SelectedStrokeWidth), SelectedStrokeWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedStrokeWidthProperty);
            Bind(nameof(SelectedBrushIndex), SelectedBrushIndexProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushIndexProperty);
            Bind(nameof(Palette), PaletteProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.PaletteProperty);
            Bind(nameof(MinStrokeWidth), MinStrokeWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MinStrokeWidthProperty);
            Bind(nameof(MaxStrokeWidth), MaxStrokeWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxStrokeWidthProperty);
            Bind(nameof(SelectedBrush), SelectedBrushProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushProperty);

            base.OnInitialized(e);
        }

        public static DependencyProperty MaxStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(MaxStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        public static DependencyProperty MinStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(MinStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        public static DependencyProperty PaletteProperty { get; } = DependencyProperty.Register(nameof(Palette), typeof(System.Collections.Generic.IList<global::Windows.UI.Xaml.Media.Brush>), typeof(InkToolbarPenButton));

        public static DependencyProperty SelectedBrushIndexProperty { get; } = DependencyProperty.Register(nameof(SelectedBrushIndex), typeof(int), typeof(InkToolbarPenButton));

        public static DependencyProperty SelectedBrushProperty { get; } = DependencyProperty.Register(nameof(SelectedBrush), typeof(global::Windows.UI.Xaml.Media.Brush), typeof(InkToolbarPenButton));

        public static DependencyProperty SelectedStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(SelectedStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        public double SelectedStrokeWidth
        {
            get => (double)GetValue(SelectedStrokeWidthProperty);
            set => SetValue(SelectedStrokeWidthProperty, value);
        }

        public int SelectedBrushIndex
        {
            get => (int)GetValue(SelectedBrushIndexProperty);
            set => SetValue(SelectedBrushIndexProperty, value);
        }

        public System.Collections.Generic.IList<global::Windows.UI.Xaml.Media.Brush> Palette
        {
            get => (System.Collections.Generic.IList<global::Windows.UI.Xaml.Media.Brush>)GetValue(PaletteProperty);
            set => SetValue(PaletteProperty, value);
        }

        public double MinStrokeWidth
        {
            get => (double)GetValue(MinStrokeWidthProperty);
            set => SetValue(MinStrokeWidthProperty, value);
        }

        public double MaxStrokeWidth
        {
            get => (double)GetValue(MaxStrokeWidthProperty);
            set => SetValue(MaxStrokeWidthProperty, value);
        }

        public global::Windows.UI.Xaml.Media.Brush SelectedBrush
        {
            get => (global::Windows.UI.Xaml.Media.Brush)GetValue(SelectedBrushProperty);
        }
    }
}