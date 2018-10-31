// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.XamlHost;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    /// <summary>
    /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbar"/>
    /// </summary>
    [ContentProperty(nameof(Children))]
    public class InkToolbar : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbar UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbar;

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbar"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbar"/>
        /// </summary>
        public InkToolbar()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbar).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbar"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbar"/>.
        /// </summary>
        protected InkToolbar(string typeName)
            : base(typeName)
        {
            Visibility = System.Windows.Visibility.Collapsed; // supports a workaround for a bug:  InkToolbar won't initialize if it's not initially collapsed.
            Children = new ObservableCollection<DependencyObject>();
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, Windows.UI.Xaml.Controls.InkToolbar.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, Windows.UI.Xaml.Controls.InkToolbar.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, Windows.UI.Xaml.Controls.InkToolbar.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, Windows.UI.Xaml.Controls.InkToolbar.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbar.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, Windows.UI.Xaml.Controls.InkToolbar.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, Windows.UI.Xaml.Controls.InkToolbar.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, Windows.UI.Xaml.Controls.InkToolbar.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, Windows.UI.Xaml.Controls.InkToolbar.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, Windows.UI.Xaml.Controls.InkToolbar.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, Windows.UI.Xaml.Controls.InkToolbar.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, Windows.UI.Xaml.Controls.InkToolbar.NameProperty);
            Bind(nameof(Tag), TagProperty, Windows.UI.Xaml.Controls.InkToolbar.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, Windows.UI.Xaml.Controls.InkToolbar.DataContextProperty);
            Bind(nameof(Width), WidthProperty, Windows.UI.Xaml.Controls.InkToolbar.WidthProperty);

            // InkToolbar specific properties
            Bind(nameof(TargetInkCanvas), TargetInkCanvasProperty, Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvasProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(IsRulerButtonChecked), IsRulerButtonCheckedProperty, Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonCheckedProperty);
            Bind(nameof(InitialControls), InitialControlsProperty, Windows.UI.Xaml.Controls.InkToolbar.InitialControlsProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(ActiveTool), ActiveToolProperty, Windows.UI.Xaml.Controls.InkToolbar.ActiveToolProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(InkDrawingAttributes), InkDrawingAttributesProperty, Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(Orientation), OrientationProperty, Windows.UI.Xaml.Controls.InkToolbar.OrientationProperty, new WindowsXamlHostWrapperConverter());
            Bind(nameof(IsStencilButtonChecked), IsStencilButtonCheckedProperty, Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedProperty);
            Bind(nameof(ButtonFlyoutPlacement), ButtonFlyoutPlacementProperty, Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacementProperty, new WindowsXamlHostWrapperConverter());

            Children.OfType<WindowsXamlHostBase>().ToList().ForEach(RelocateChildToUwpControl);

            UwpControl.ActiveToolChanged += OnActiveToolChanged;
            UwpControl.EraseAllClicked += OnEraseAllClicked;
            UwpControl.InkDrawingAttributesChanged += OnInkDrawingAttributesChanged;
            UwpControl.IsRulerButtonCheckedChanged += OnIsRulerButtonCheckedChanged;
            UwpControl.IsStencilButtonCheckedChanged += OnIsStencilButtonCheckedChanged;
            UwpControl.Loaded += OnUwpControlLoaded;
            base.OnInitialized(e);
        }

        /// <summary>
        /// supports a workaround for a bug:  InkToolbar won't initialize if it's not initially collapsed, so we update visibility on it's loaded event.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event parameters</param>
        private void OnUwpControlLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Visible;
            UwpControl.Loaded -= OnUwpControlLoaded;
        }

        private void RelocateChildToUwpControl(WindowsXamlHostBase obj)
        {
            if (obj.GetUwpInternalObject() is Windows.UI.Xaml.UIElement child)
            {
                UwpControl.Children.Add(child);
            }
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.ActiveToolProperty"/>
        /// </summary>
        public static DependencyProperty ActiveToolProperty { get; } = DependencyProperty.Register(nameof(ActiveTool), typeof(WindowsXamlHostBase), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.InitialControlsProperty"/>
        /// </summary>
        public static DependencyProperty InitialControlsProperty { get; } = DependencyProperty.Register(nameof(InitialControls), typeof(InkToolbarInitialControls), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesProperty"/>
        /// </summary>
        public static DependencyProperty InkDrawingAttributesProperty { get; } = DependencyProperty.Register(nameof(InkDrawingAttributes), typeof(InkDrawingAttributes), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonCheckedProperty"/>
        /// </summary>
        public static DependencyProperty IsRulerButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsRulerButtonChecked), typeof(bool), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvasProperty"/>
        /// </summary>
        public static DependencyProperty TargetInkCanvasProperty { get; } = DependencyProperty.Register(nameof(TargetInkCanvas), typeof(InkCanvas), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacementProperty"/>
        /// </summary>
        public static DependencyProperty ButtonFlyoutPlacementProperty { get; } = DependencyProperty.Register(nameof(ButtonFlyoutPlacement), typeof(InkToolbarButtonFlyoutPlacement), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedProperty"/>
        /// </summary>
        public static DependencyProperty IsStencilButtonCheckedProperty { get; } = DependencyProperty.Register(nameof(IsStencilButtonChecked), typeof(bool), typeof(InkToolbar));

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.OrientationProperty"/>
        /// </summary>
        public static DependencyProperty OrientationProperty { get; } = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(InkToolbar));

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.GetToolButton"/>
        /// </summary>
        /// <returns>WindowsXamlHostBase</returns>
        public WindowsXamlHostBase GetToolButton(InkToolbarTool tool) => UwpControl.GetToolButton((Windows.UI.Xaml.Controls.InkToolbarTool)(int)tool).GetWrapper();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.GetToggleButton"/>
        /// </summary>
        /// <returns>InkToolbarToggleButton</returns>
        public InkToolbarToggleButton GetToggleButton(InkToolbarToggle tool) => (InkToolbarToggleButton)UwpControl.GetToggleButton((Windows.UI.Xaml.Controls.InkToolbarToggle)(int)tool).GetWrapper();

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.GetMenuButton"/>
        /// </summary>
        /// <returns>InkToolbarMenuButton</returns>
        public InkToolbarMenuButton GetMenuButton(InkToolbarMenuKind menu) => (InkToolbarMenuButton)UwpControl.GetMenuButton((Windows.UI.Xaml.Controls.InkToolbarMenuKind)(int)menu).GetWrapper();

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvas"/>
        /// </summary>
        public InkCanvas TargetInkCanvas
        {
            get => (InkCanvas)GetValue(TargetInkCanvasProperty);
            set => SetValue(TargetInkCanvasProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonChecked"/>
        /// </summary>
        public bool IsRulerButtonChecked
        {
            get => (bool)GetValue(IsRulerButtonCheckedProperty);
            set => SetValue(IsRulerButtonCheckedProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.InitialControls"/>
        /// </summary>
        public InkToolbarInitialControls InitialControls
        {
            get => (InkToolbarInitialControls)GetValue(InitialControlsProperty);
            set => SetValue(InitialControlsProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.ActiveTool"/>
        /// </summary>
        public WindowsXamlHostBase ActiveTool
        {
            get => (WindowsXamlHostBase)GetValue(ActiveToolProperty);
            set => SetValue(ActiveToolProperty, value);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributes"/>
        /// </summary>
        public InkDrawingAttributes InkDrawingAttributes
        {
            get => (InkDrawingAttributes)GetValue(InkDrawingAttributesProperty);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.Orientation"/>
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonChecked"/>
        /// </summary>
        public bool IsStencilButtonChecked
        {
            get => (bool)GetValue(IsStencilButtonCheckedProperty);
            set => SetValue(IsStencilButtonCheckedProperty, value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacement"/>
        /// </summary>
        public InkToolbarButtonFlyoutPlacement ButtonFlyoutPlacement
        {
            get => (InkToolbarButtonFlyoutPlacement)GetValue(ButtonFlyoutPlacementProperty);
            set => SetValue(ButtonFlyoutPlacementProperty, value);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.ActiveToolChanged"/>
        /// </summary>
        public event EventHandler<object> ActiveToolChanged = (sender, args) => { };

        private void OnActiveToolChanged(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            ActiveToolChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.EraseAllClicked"/>
        /// </summary>
        public event EventHandler<object> EraseAllClicked = (sender, args) => { };

        private void OnEraseAllClicked(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            EraseAllClicked?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesChanged"/>
        /// </summary>
        public event EventHandler<object> InkDrawingAttributesChanged = (sender, args) => { };

        private void OnInkDrawingAttributesChanged(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            InkDrawingAttributesChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonCheckedChanged"/>
        /// </summary>
        public event EventHandler<object> IsRulerButtonCheckedChanged = (sender, args) => { };

        private void OnIsRulerButtonCheckedChanged(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            IsRulerButtonCheckedChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedChanged"/>
        /// </summary>
        public event EventHandler<InkToolbarIsStencilButtonCheckedChangedEventArgs> IsStencilButtonCheckedChanged = (sender, args) => { };

        private void OnIsStencilButtonCheckedChanged(Windows.UI.Xaml.Controls.InkToolbar sender, Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            IsStencilButtonCheckedChanged?.Invoke(this, args);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<DependencyObject> Children
        {
            get; set;
        }
    }
}