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
<<<<<<< HEAD
=======
    [TemplateVisualState(Name = StateContentExpanded, GroupName = ExpandedGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsed, GroupName = ExpandedGroupStateContent)]
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
    [TemplateVisualState(Name = StateContentLeftDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentDownDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentRightDirection, GroupName = ExpandDirectionGroupStateContent)]
    [TemplateVisualState(Name = StateContentUpDirection, GroupName = ExpandDirectionGroupStateContent)]
<<<<<<< HEAD
    [TemplateVisualState(Name = StateContentVisibleLeft, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentVisibleDown, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentVisibleRight, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentVisibleUp, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedLeft, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedDown, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedRight, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentCollapsedUp, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentOverlayLeft, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentOverlayDown, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentOverlayRight, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplateVisualState(Name = StateContentOverlayUp, GroupName = DisplayModeAndDirectionStatesGroupStateContent)]
    [TemplatePart(Name = RootGridPart, Type = typeof(Grid))]
    [TemplatePart(Name = ExpanderToggleButtonPart, Type = typeof(ToggleButton))]
    [TemplatePart(Name = LayoutTransformerPart, Type = typeof(LayoutTransformControl))]
    [TemplatePart(Name = ContentOverlayPart, Type = typeof(ContentPresenter))]
=======
    [TemplatePart(Name = RootGridPart, Type = typeof(Grid))]
    [TemplatePart(Name = ExpanderToggleButtonPart, Type = typeof(ToggleButton))]
    [TemplatePart(Name = LayoutTransformerPart, Type = typeof(LayoutTransformControl))]
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
=======
            if (IsExpanded)
            {
                VisualStateManager.GoToState(this, StateContentExpanded, false);
            }
            else
            {
                VisualStateManager.GoToState(this, StateContentCollapsed, false);
            }

>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);

            if (button != null)
            {
                button.KeyDown -= ExpanderToggleButtonPart_KeyDown;
                button.KeyDown += ExpanderToggleButtonPart_KeyDown;
            }

            OnExpandDirectionChanged();
<<<<<<< HEAD
            OnDisplayModeOrIsExpandedChanged(false);
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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

<<<<<<< HEAD
        /// <summary>
        /// Called when the ExpandDirection on Expander changes
        /// </summary>
        private void OnExpandDirectionChanged()
=======
        public void OnExpandDirectionChanged()
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
        {
            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);

            if (button == null)
            {
                return;
            }

<<<<<<< HEAD
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
=======
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
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
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
<<<<<<< HEAD

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
                    visualState = GetDisplayModeVisualState(StateContentOverlayLeft, StateContentCollapsedLeft, StateContentVisibleLeft);
                    break;
                case ExpandDirection.Down:
                    visualState = GetDisplayModeVisualState(StateContentOverlayDown, StateContentCollapsedDown, StateContentVisibleDown);
                    break;
                case ExpandDirection.Right:
                    visualState = GetDisplayModeVisualState(StateContentOverlayRight, StateContentCollapsedRight, StateContentVisibleRight);
                    break;
                case ExpandDirection.Up:
                    visualState = GetDisplayModeVisualState(StateContentOverlayUp, StateContentCollapsedUp, StateContentVisibleUp);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(visualState))
            {
                VisualStateManager.GoToState(this, visualState, true);
            }
        }

        private string GetDisplayModeVisualState(string overlayState, string collapsedState, string visibleState)
        {
            if (!IsExpanded && DisplayMode == ExpanderDisplayMode.Overlay)
            {
                // Overlay
                return overlayState;
            }

            if (!IsExpanded && DisplayMode == ExpanderDisplayMode.Expand)
            {
                // Collapsed
                return collapsedState;
            }

            // Visible
            return visibleState;
        }
=======
>>>>>>> fb2912293936b8803e6224af5086e6d0c8780bcd
    }
}