using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    public partial class ViewportBehavior
    {
        public static readonly DependencyProperty IsAlwaysOnProperty =
            DependencyProperty.Register(nameof(IsAlwaysOn), typeof(bool), typeof(ViewportBehavior), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether this behavior will remain attached after the associated element enters the viewport. When false, the behavior will remove itself after entering.
        /// </summary>
        public bool IsAlwaysOn
        {
            get { return (bool)GetValue(IsAlwaysOnProperty); }
            set { SetValue(IsAlwaysOnProperty, value); }
        }
    }
}
