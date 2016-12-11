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

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="Expander"/> control allows user to show/hide content based on a boolean state
    /// </summary>
    [TemplateVisualState(Name = StateContentExpanded, GroupName = GroupContent)]
    [TemplateVisualState(Name = StateContentCollapsed, GroupName = GroupContent)]
    [TemplatePart(Name = ExpanderToggleButtonPart, Type = typeof(ToggleButton))]
    [TemplatePart(Name = HeaderButtonPart, Type = typeof(ButtonBase))]
    [ContentProperty(Name = "Content")]
    public partial class Expander : Control
    {
        private ToggleButton _expanderButton;
        private ButtonBase _headerButton;

        private RowDefinition _mainContentRow;

        public Expander()
        {
            DefaultStyleKey = typeof(Expander);
        }

        protected override void OnApplyTemplate()
        {
            _expanderButton = GetTemplateChild(ExpanderToggleButtonPart) as ToggleButton;
            _headerButton = GetTemplateChild(HeaderButtonPart) as ButtonBase;
            _mainContentRow = GetTemplateChild(MainContentRowPart) as RowDefinition;

            if (_expanderButton != null)
            {
                _expanderButton.Checked += OnExpanderButtonChecked;
                _expanderButton.Unchecked += OnExpanderButtonUnChecked;
                _expanderButton.IsChecked = IsExpanded;
                if (IsExpanded)
                {
                    ExpandControl();
                }
                else
                {
                    CollapseControl();
                }
            }

            if (_headerButton != null)
            {
                _headerButton.Click += OnHeaderButtonClick;
            }
        }

        private void OnHeaderButtonClick(object sender, RoutedEventArgs e)
        {
            HeaderButtonCommand?.Execute(null);
        }

        private void ExpandControl()
        {
            if (_mainContentRow == null || !_mainContentRow.Height.Value.Equals(0d))
            {
                return;
            }

            VisualStateManager.GoToState(this, StateContentExpanded, true);
            _mainContentRow.Height = new GridLength(1, GridUnitType.Auto);
        }

        private void CollapseControl()
        {
            if (_mainContentRow == null || _mainContentRow.Height.Value.Equals(0d))
            {
                return;
            }

            VisualStateManager.GoToState(this, StateContentCollapsed, true);
            _mainContentRow.Height = new GridLength(0d);
        }

        private void OnExpanderButtonUnChecked(object sender, RoutedEventArgs e)
        {
            IsExpanded = false;
            CollapseControl();
        }

        private void OnExpanderButtonChecked(object sender, RoutedEventArgs e)
        {
            IsExpanded = true;
            ExpandControl();
        }
    }
}