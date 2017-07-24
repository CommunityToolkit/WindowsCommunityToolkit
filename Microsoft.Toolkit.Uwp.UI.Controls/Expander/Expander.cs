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
    [TemplateVisualState(Name = StateContentLeftOrientation, GroupName = OrientationGroupStateContent)]
    [TemplateVisualState(Name = StateContentTopOrientation, GroupName = OrientationGroupStateContent)]
    [TemplateVisualState(Name = StateContentRightOrientation, GroupName = OrientationGroupStateContent)]
    [TemplateVisualState(Name = StateContentBottomOrientation, GroupName = OrientationGroupStateContent)]
    [TemplatePart(Name = RootGridPart, Type = typeof(Grid))]
    [TemplatePart(Name = ExpanderToggleButtonPart, Type = typeof(ToggleButton))]
    [TemplatePart(Name = LayoutTransformerPart, Type = typeof(LayoutTransformer))]
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

            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);

            if (button != null)
            {
                button.KeyDown -= ExpanderToggleButtonPart_KeyDown;
                button.KeyDown += ExpanderToggleButtonPart_KeyDown;
            }

            OnOrientationChanged();
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

        public void OnOrientationChanged()
        {
            var button = (ToggleButton)GetTemplateChild(ExpanderToggleButtonPart);
            var layoutTransformer = (LayoutTransformer)GetTemplateChild(LayoutTransformerPart);

            if (button == null || layoutTransformer == null)
            {
                return;
            }

            if (Orientation == ExpanderOrientation.Left)
            {
                VisualStateManager.GoToState(this, StateContentLeftOrientation, true);
                VisualStateManager.GoToState(button, StateContentLeftOrientation, true);
            }

            if (Orientation == ExpanderOrientation.Top)
            {
                VisualStateManager.GoToState(this, StateContentTopOrientation, true);
                VisualStateManager.GoToState(button, StateContentTopOrientation, true);
            }

            if (Orientation == ExpanderOrientation.Right)
            {
                VisualStateManager.GoToState(this, StateContentRightOrientation, true);
                VisualStateManager.GoToState(button, StateContentRightOrientation, true);
            }

            if (Orientation == ExpanderOrientation.Bottom)
            {
                VisualStateManager.GoToState(this, StateContentBottomOrientation, true);
                VisualStateManager.GoToState(button, StateContentBottomOrientation, true);
            }

            // Apply rotation on expander toggle button
            layoutTransformer.ApplyLayoutTransform();

            // Re-execute animation on expander toggle button (to set correct arrow rotation)
            VisualStateManager.GoToState(button, "Normal", true);
            if (button.IsChecked.HasValue && button.IsChecked.Value)
            {
                VisualStateManager.GoToState(button, "Checked", true);
            }
        }
    }
}