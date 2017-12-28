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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="Expander"/> control allows user to show/hide content based on a boolean state
    /// </summary>
    [TemplateVisualState(Name = StateContentLeftDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentDownDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentRightDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentUpDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentVisibleLeft, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentVisibleDown, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentVisibleRight, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentVisibleUp, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedLeft, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedDown, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedRight, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedUp, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplatePart(Name = RootGridPart, Type = typeof(Grid))]
    [TemplatePart(Name = ExpanderToggleButtonPart, Type = typeof(ToggleButton))]
    [TemplatePart(Name = LayoutTransformerPart, Type = typeof(LayoutTransformControl))]
    [TemplatePart(Name = ContentOverlayPart, Type = typeof(ContentPresenter))]
    [ContentProperty(Name = "Content")]
    public partial class Expander : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Expander"/> class.
        /// </summary>
        public Expander()
        {
            DefaultStyleKey = typeof(Expander);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);

            if (button != null)
            {
                button.KeyDown -= ExpanderToggleButtonPart_KeyDown;
                button.KeyDown += ExpanderToggleButtonPart_KeyDown;
            }

            OnExpandDirectionChanged();
            OnDisplayModeOrIsExpandedChanged(false);
        }

        /// <summary>
        /// Called when control is expanded
        /// </summary>
        /// <param name="args">EventArgs</param>
        protected virtual void OnExpanded(EventArgs args)
        {
            Expanded?.Invoke(this, args);
        }

        /// <summary>
        /// Called when control is collapsed
        /// </summary>
        /// <param name="args">EventArgs</param>
        protected virtual void OnCollapsed(EventArgs args)
        {
            Collapsed?.Invoke(this, args);
        }

        private void ExpanderToggleButtonPart_KeyDown(object sender, KeyRoutedEventArgs e)
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
            OnDisplayModeOrIsExpandedChanged();
            OnExpanded(EventArgs.Empty);
        }

        private void CollapseControl()
        {
            OnDisplayModeOrIsExpandedChanged();
            OnCollapsed(EventArgs.Empty);
        }

        /// <summary>
        /// Called when the ExpandDirection on Expander changes
        /// </summary>
        private void OnExpandDirectionChanged()
        {
            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);

            if (button == null)
            {
                return;
            }

            UpdateDisplayModeOrExpanderDirection();

            switch (ExpandDirection)
            {
                case ExpandDirection.Left:
                    VisualStateManager.GoToState(button, StateContentLeftDirection, true);
                    break;
                case ExpandDirection.Down:
                    VisualStateManager.GoToState(button, StateContentDownDirection, true);
                    break;
                case ExpandDirection.Right:
                    VisualStateManager.GoToState(button, StateContentRightDirection, true);
                    break;
                case ExpandDirection.Up:
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

        private void OnDisplayModeOrIsExpandedChanged(bool useTransitions = true)
        {
            UpdateDisplayModeOrExpanderDirection();
        }

        private void UpdateDisplayModeOrExpanderDirection()
        {
            string visualState = null;

            switch (ExpandDirection)
            {
                case ExpandDirection.Left:
                    visualState = GetDisplayModeVisualState(StateContentCollapsedLeft, StateContentVisibleLeft);
                    break;
                case ExpandDirection.Down:
                    visualState = GetDisplayModeVisualState(StateContentCollapsedDown, StateContentVisibleDown);
                    break;
                case ExpandDirection.Right:
                    visualState = GetDisplayModeVisualState(StateContentCollapsedRight, StateContentVisibleRight);
                    break;
                case ExpandDirection.Up:
                    visualState = GetDisplayModeVisualState(StateContentCollapsedUp, StateContentVisibleUp);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(visualState))
            {
                VisualStateManager.GoToState(this, visualState, true);
            }
        }

        private string GetDisplayModeVisualState(string collapsedState, string visibleState)
        {
            return IsExpanded ? visibleState : collapsedState;
        }
    }
}