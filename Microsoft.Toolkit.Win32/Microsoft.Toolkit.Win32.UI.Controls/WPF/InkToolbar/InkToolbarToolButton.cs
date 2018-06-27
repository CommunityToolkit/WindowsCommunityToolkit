using System;
using System.Windows;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkToolbarToolButton : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.InkToolbarToolButton UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.InkToolbarToolButton;

        public InkToolbarToolButton()
            : this("Windows.UI.Xaml.Controls.InkToolbarToolButton")
        {
        }

        // Summary:
        //     Initializes a new instance of the InkToolbarToolButton class.
        public InkToolbarToolButton(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.WidthProperty);

            // InkToolbarToolButton specific properties
            Bind(nameof(IsExtensionGlyphShown), IsExtensionGlyphShownProperty, global::Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShownProperty);

            base.OnInitialized(e);
        }

        public static DependencyProperty IsExtensionGlyphShownProperty { get; } = DependencyProperty.Register(nameof(IsExtensionGlyphShown), typeof(bool), typeof(InkToolbarToolButton));

        public bool IsExtensionGlyphShown
        {
            get => (bool)GetValue(IsExtensionGlyphShownProperty);
            set => SetValue(IsExtensionGlyphShownProperty, value);
        }

        public global::Windows.UI.Xaml.Controls.InkToolbarTool ToolKind
        {
            get => UwpControl.ToolKind;
        }
    }
}