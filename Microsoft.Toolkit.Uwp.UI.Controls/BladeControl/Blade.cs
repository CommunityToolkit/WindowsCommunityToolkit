using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The Blade is used as a child in the BladeControl
    /// </summary>
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    public partial class Blade : Control
    {
        private Button _closeButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="Blade"/> class.
        /// </summary>
        public Blade()
        {
            DefaultStyleKey = typeof(Blade);
            BorderBrush = new SolidColorBrush(Colors.DarkGray);
            BorderThickness = new Thickness(1);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _closeButton = GetTemplateChild("CloseButton") as Button;

            if (_closeButton == null)
            {
                return;
            }

            _closeButton.Tapped -= CloseButtonOnTap;
            _closeButton.Tapped += CloseButtonOnTap;
        }

        private void CloseButtonOnTap(object sender, RoutedEventArgs routedEventArgs)
        {
            IsOpen = false;
        }
    }
}