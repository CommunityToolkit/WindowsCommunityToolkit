using System;
using System.Collections.Generic;
using System.Windows;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkToolbarPenButton : InkToolbarToolButton
    {
        public InkToolbarPenButton()
            : this("Windows.UI.Xaml.Controls.InkToolbarPenButton")
        {
        }

        public InkToolbarPenButton(string childType)
            : base(childType)
        {
            Bind(nameof(SelectedStrokeWidth), SelectedStrokeWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedStrokeWidthProperty);
            Bind(nameof(SelectedBrushIndex), SelectedBrushIndexProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushIndexProperty);
            Bind(nameof(Palette), PaletteProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.PaletteProperty);
            Bind(nameof(MinStrokeWidth), MinStrokeWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MinStrokeWidthProperty);
            Bind(nameof(MaxStrokeWidth), MaxStrokeWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.MaxStrokeWidthProperty);
            Bind(nameof(SelectedBrush), SelectedBrushProperty, global::Windows.UI.Xaml.Controls.InkToolbarPenButton.SelectedBrushProperty);
        }

        public static DependencyProperty MaxStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(MaxStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        public static DependencyProperty MinStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(MinStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        public static DependencyProperty PaletteProperty { get; } = DependencyProperty.Register(nameof(Palette), typeof(IList<Brush>), typeof(InkToolbarPenButton));

        public static DependencyProperty SelectedBrushIndexProperty { get; } = DependencyProperty.Register(nameof(SelectedBrushIndex), typeof(int), typeof(InkToolbarPenButton));

        public static DependencyProperty SelectedBrushProperty { get; } = DependencyProperty.Register(nameof(SelectedBrush), typeof(Brush), typeof(InkToolbarPenButton));

        public static DependencyProperty SelectedStrokeWidthProperty { get; } = DependencyProperty.Register(nameof(SelectedStrokeWidth), typeof(double), typeof(InkToolbarPenButton));

        public double SelectedStrokeWidth { get => (double)GetValue(SelectedStrokeWidthProperty); set => SetValue(SelectedStrokeWidthProperty, value); }

        public int SelectedBrushIndex { get => (int)GetValue(SelectedBrushIndexProperty); set => SetValue(SelectedBrushIndexProperty, value); }

        public IList<Brush> Palette { get => (IList<Brush>)GetValue(PaletteProperty); set => SetValue(PaletteProperty, value); }

        public double MinStrokeWidth { get => (double)GetValue(MinStrokeWidthProperty); set => SetValue(MinStrokeWidthProperty, value); }

        public double MaxStrokeWidth { get => (double)GetValue(MaxStrokeWidthProperty); set => SetValue(MaxStrokeWidthProperty, value); }

        public Brush SelectedBrush { get => (Brush)GetValue(SelectedBrushProperty); }
    }
}
