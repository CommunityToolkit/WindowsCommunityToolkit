using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines a control for providing a header for read-only text.
    /// </summary>
    [TemplatePart(Name = "HeaderContent", Type = typeof(TextBlock))]
    public partial class HeaderedTextBlock : Control
    {
        private TextBlock _headerContent;
        private TextBlock _textContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderedTextBlock"/> class.
        /// </summary>
        public HeaderedTextBlock()
        {
            DefaultStyleKey = typeof(HeaderedTextBlock);
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _headerContent = GetTemplateChild("HeaderContent") as TextBlock;
            _textContent = GetTemplateChild("TextContent") as TextBlock;

            UpdateVisibility();
        }

        private void UpdateHeader()
        {
            if (_headerContent != null)
            {
                UpdateVisibility();
            }
        }

        private void UpdateText()
        {
            if (_textContent != null)
            {
                UpdateVisibility();
            }
        }

        private void UpdateVisibility()
        {
            if (_headerContent != null)
            {
                _headerContent.Visibility = string.IsNullOrWhiteSpace(_headerContent.Text)
                                                     ? Visibility.Collapsed
                                                     : Visibility.Visible;
            }

            if (_textContent != null)
            {
                _textContent.Visibility = string.IsNullOrWhiteSpace(_textContent.Text)
                                                    ? Visibility.Collapsed
                                                    : Visibility.Visible;
            }
        }

        private void UpdateForOrientation(Orientation orientationValue)
        {
            switch (orientationValue)
            {
                case Orientation.Vertical:
                    VisualStateManager.GoToState(this, "Vertical", true);
                    break;
                case Orientation.Horizontal:
                    VisualStateManager.GoToState(this, "Horizontal", true);
                    break;
            }
        }
    }
}