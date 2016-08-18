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
    /// Represents a control that displays a header that has a collapsible window that displays content.
    /// </summary>
    [TemplateVisualState(Name = "ExpandedDown", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "CollapsedDown", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "ExpandedUp", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "CollapsedUp", GroupName = "CommonStates")]
    [TemplatePart(Name = "ExpanderButton", Type = typeof(Button))]
    [TemplatePart(Name = "HeaderContentPresenter", Type = typeof(ContentPresenter))]
    public partial class Expander : ContentControl
    {
        private ContentPresenter _headerContentPresenter;
        private Button _expanderButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="Expander"/> class.
        /// </summary>
        public Expander()
        {
            DefaultStyleKey = typeof(Expander);
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_expanderButton != null)
            {
                _expanderButton.Click -= ExpanderButton_Click;
            }

            _expanderButton = GetTemplateChild("ExpanderButton") as Button;
            _headerContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;

            if (_expanderButton != null)
            {
                _expanderButton.Click += ExpanderButton_Click;
            }

        }

        private void ExpanderButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsExpanded = !this.IsExpanded;
            if (this.IsExpanded)
            {
                if (this.Expanded != null)
                {
                    this.Expanded.Invoke(this, new RoutedEventArgs());
                }
            }
            else
            {
                if (this.Collapsed != null)
                {
                    this.Collapsed.Invoke(this, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// Updates the visual state based on changes to ExpandDirection and IsExpanded
        /// </summary>
        private void UpdateVisualState()
        {
            switch (this.ExpandDirection)
            {
                case ExpandDirection.Down:
                    {
                        VisualStateManager.GoToState(this, this.IsExpanded ? "ExpandedDown" : "CollapsedDown", true);
                        break;
                    }

                case ExpandDirection.Up:
                    {
                        VisualStateManager.GoToState(this, this.IsExpanded ? "ExpandedUp" : "CollapsedUp", true);
                        break;
                    }

                case ExpandDirection.Right:
                    {
                        VisualStateManager.GoToState(this, this.IsExpanded ? "ExpandedRight" : "CollapsedRight", true);
                        break;
                    }
            }
        }

    }
}
