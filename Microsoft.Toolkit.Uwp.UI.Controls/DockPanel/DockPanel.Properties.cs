using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines an area where you can arrange child elements either horizontally or vertically, relative to each other.
    /// </summary>
    public partial class DockPanel
    {
        private const string DockName = "Dock";

        /// <summary>
        /// Gets or sets a value that indicates the position of a child element within a parent <see cref="DockPanel"/>.
        /// </summary>
        public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached(
            DockName,
            typeof(Dock),
            typeof(FrameworkElement),
            new PropertyMetadata(Dock.Left, DockChanged));

        /// <summary>
        /// Gets DockProperty attached property
        /// </summary>
        /// <param name="obj">Target FrameworkElement</param>
        /// <returns>Dock value</returns>
        public static Dock GetDock(FrameworkElement obj)
        {
            return (Dock)obj.GetValue(DockProperty);
        }

        /// <summary>
        /// Sets DockProperty attached property
        /// </summary>
        /// <param name="obj">Target FrameworkElement</param>
        /// <param name="value">Dock Value</param>
        public static void SetDock(FrameworkElement obj, Dock value)
        {
            obj.SetValue(DockProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="LastChildFill"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastChildFillProperty
            = DependencyProperty.Register(
                nameof(LastChildFill),
                typeof(bool),
                typeof(DockPanel),
                new PropertyMetadata(true, LastChildFillChanged));

        /// <summary>
        /// Gets or sets a value that indicates whether the last child element within a DockPanel stretches to fill the remaining available space.
        /// </summary>
        public bool LastChildFill
        {
            get { return (bool)GetValue(LastChildFillProperty); }
            set { SetValue(LastChildFillProperty, value); }
        }
    }
}
