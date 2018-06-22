using System;
using System.Windows;
using Microsoft.Windows.Interop;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using uwpControls = global::Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkToolbarToolButton : WindowsXamlHost
    {
        protected uwpControls.InkToolbarToolButton UwpControl => this.XamlRoot as uwpControls.InkToolbarToolButton;

        // Summary:
        //     Initializes a new instance of the InkToolbarToolButton class.
        public InkToolbarToolButton()
            : this("Windows.UI.Xaml.Controls.InkToolbarToolButton")
        {
        }

        public InkToolbarToolButton(string typeName)
            : base(typeName)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, uwpControls.InkToolbarToolButton.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, uwpControls.InkToolbarToolButton.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, uwpControls.InkToolbarToolButton.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, uwpControls.InkToolbarToolButton.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, uwpControls.InkToolbarToolButton.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, uwpControls.InkToolbarToolButton.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, uwpControls.InkToolbarToolButton.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, uwpControls.InkToolbarToolButton.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, uwpControls.InkToolbarToolButton.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, uwpControls.InkToolbarToolButton.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, uwpControls.InkToolbarToolButton.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, uwpControls.InkToolbarToolButton.NameProperty);
            Bind(nameof(Tag), TagProperty, uwpControls.InkToolbarToolButton.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, uwpControls.InkToolbarToolButton.DataContextProperty);
            Bind(nameof(Width), WidthProperty, uwpControls.InkToolbarToolButton.WidthProperty);

            // RadioButton specific properties
            Bind(nameof(GroupName), GroupNameProperty, uwpControls.InkToolbarToolButton.GroupNameProperty);

            // InkToolbarToolButton specific properties
            Bind(nameof(IsExtensionGlyphShown), IsExtensionGlyphShownProperty, uwpControls.InkToolbarToolButton.IsExtensionGlyphShownProperty);
        }

        public static DependencyProperty GroupNameProperty { get; } = DependencyProperty.Register(nameof(GroupNameProperty), typeof(string), typeof(InkToolbarToolButton));

        public static DependencyProperty IsExtensionGlyphShownProperty { get; } = DependencyProperty.Register(nameof(IsExtensionGlyphShown), typeof(bool), typeof(InkToolbarToolButton));

        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty); set => SetValue(GroupNameProperty, value);
        }

        public bool IsExtensionGlyphShown
        {
            get => (bool)GetValue(IsExtensionGlyphShownProperty); set => SetValue(IsExtensionGlyphShownProperty, value);
        }

        public InkToolbarTool ToolKind { get => UwpControl.ToolKind; }
    }
}