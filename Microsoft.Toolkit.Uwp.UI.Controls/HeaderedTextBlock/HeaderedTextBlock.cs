// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines a control for providing a header for read-only text.
    /// </summary>
    [TemplatePart(Name = "HeaderContentPresenter", Type = typeof(ContentPresenter))]
    public partial class HeaderedTextBlock : Control
    {
        private ContentPresenter _headerContentPresenter;
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

            _headerContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
            _textContent = GetTemplateChild("TextContent") as TextBlock;

            UpdateVisibility();
            UpdateForOrientation(this.Orientation);
        }

        private void UpdateVisibility()
        {
            if (_headerContentPresenter != null)
            {
                _headerContentPresenter.Visibility = _headerContentPresenter.Content == null
                                                     ? Visibility.Collapsed
                                                     : Visibility.Visible;
            }

            if (_textContent != null)
            {
                _textContent.Visibility = string.IsNullOrWhiteSpace(_textContent.Text) && HideTextIfEmpty
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