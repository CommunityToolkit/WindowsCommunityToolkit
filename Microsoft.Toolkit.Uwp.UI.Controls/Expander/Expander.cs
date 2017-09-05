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

using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="Expander"/> control allows user to show/hide content based on a boolean state
    /// </summary>
    [TemplateVisualState(Name = StateContentExpanded, GroupName = ExpandedGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsed, GroupName = ExpandedGroupStateContent)]
    [TemplateVisualState(Name = StateContentLeftDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentDownDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentRightDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentUpDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplatePart(Name = RootGridPart, Type = typeof(Grid))]
    [TemplatePart(Name = ExpanderToggleButtonPart, Type = typeof(ToggleButton))]
    [TemplatePart(Name = LayoutTransformerPart, Type = typeof(LayoutTransformControl))]
    [ContentProperty(Name = "Content")]
    public partial class Expander : ContentControl
    {
        public Expander()
        {
            DefaultStyleKey = typeof(Expander);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (IsExpanded)
            {
                VisualStateManager.GoToState(this, StateContentExpanded, false);
            }
            else
            {
                VisualStateManager.GoToState(this, StateContentCollapsed, false);
            }

            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);

            if (button != null)
            {
                button.KeyDown -= ExpanderToggleButtonPart_KeyDown;
                button.KeyDown += ExpanderToggleButtonPart_KeyDown;
            }

            OnExpandDirectionChanged();
        }

        protected virtual void OnExpanded(EventArgs args)
        {
            Expanded?.Invoke(this, args);
        }

        protected virtual void OnCollapsed(EventArgs args)
        {
            Collapsed?.Invoke(this, args);
        }

        private void ExpanderToggleButtonPart_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
            {
                return;
            }

            var button = sender as ToggleButton;

            if (button == null)
            {
                return;
            }

            IsExpanded = !IsExpanded;

            e.Handled = true;
        }

        private void ExpandControl()
        {
            VisualStateManager.GoToState(this, StateContentExpanded, true);
            OnExpanded(EventArgs.Empty);
        }

        private void CollapseControl()
        {
            VisualStateManager.GoToState(this, StateContentCollapsed, true);
            OnCollapsed(EventArgs.Empty);
        }

        public void OnExpandDirectionChanged()
        {
            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);

            if (button == null)
            {
                return;
            }

            switch (ExpandDirection)
            {
                case ExpandDirection.Left:
                    VisualStateManager.GoToState(this, StateContentLeftDirection, true);
                    VisualStateManager.GoToState(button, StateContentLeftDirection, true);
                    break;
                case ExpandDirection.Down:
                    VisualStateManager.GoToState(this, StateContentDownDirection, true);
                    VisualStateManager.GoToState(button, StateContentDownDirection, true);
                    break;
                case ExpandDirection.Right:
                    VisualStateManager.GoToState(this, StateContentRightDirection, true);
                    VisualStateManager.GoToState(button, StateContentRightDirection, true);
                    break;
                case ExpandDirection.Up:
                    VisualStateManager.GoToState(this, StateContentUpDirection, true);
                    VisualStateManager.GoToState(button, StateContentUpDirection, true);
                    break;
            }

            // Re-execute animation on expander toggle button (to set correct arrow rotation)
            VisualStateManager.GoToState(button, "Normal", true);
            if (button.IsChecked.HasValue && button.IsChecked.Value)
            {
                VisualStateManager.GoToState(button, "Checked", true);
            }
        }
    }
}