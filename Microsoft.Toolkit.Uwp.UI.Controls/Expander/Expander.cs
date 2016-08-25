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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents a control that displays a header that has a collapsible window that displays content.
    /// </summary>
    [TemplatePart(Name = EXPANDERBUTTON, Type = typeof(Button))]
    [TemplatePart(Name = ANIMATEINCONTENTDOWN, Type = typeof(Storyboard))]
    [TemplatePart(Name = ANIMATEINCONTENTUP, Type = typeof(Storyboard))]
    [TemplatePart(Name = ANIMATEINCONTENTLEFT, Type = typeof(Storyboard))]
    [TemplatePart(Name = ANIMATEINCONTENTRIGHT, Type = typeof(Storyboard))]
    public partial class Expander : ContentControl
    {
        private const string ANIMATEINCONTENTDOWN = "AnimateInContentDown";
        private const string ANIMATEINCONTENTUP = "AnimateInContentUp";
        private const string ANIMATEINCONTENTLEFT = "AnimateInContentLeft";
        private const string ANIMATEINCONTENTRIGHT = "AnimateInContentRight";
        private const string EXPANDERBUTTON = "ExpanderButton";
        private const string HEADERROOT = "HeaderRoot";

        private const string VSEXPANDEDUP = "ExpandedUp";
        private const string VSEXPANDEDDOWN = "ExpandedDown";
        private const string VSEXPANDEDLEFT = "ExpandedLeft";
        private const string VSEXPANDEDRIGHT = "ExpandedRight";

        private const string VSCOLLAPSEDUP = "CollapsedUp";
        private const string VSCOLLAPSEDDOWN = "CollapsedDown";
        private const string VSCOLLAPSEDLEFT = "CollapsedLeft";
        private const string VSCOLLAPSEDRIGHT = "CollapsedRight";

        private ToggleButton _headerRoot;

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

            if (_headerRoot != null)
            {
                _headerRoot.Checked -= HeaderRoot_Checked;
                _headerRoot.Unchecked -= HeaderRoot_Unchecked;
            }

            _headerRoot = GetTemplateChild(HEADERROOT) as ToggleButton;

            if (_headerRoot != null)
            {
                if (IsExpanded)
                {
                    _headerRoot.IsChecked = true;
                }

                var b = new Binding() { Path = new PropertyPath("IsChecked"), Source = _headerRoot, Mode = BindingMode.TwoWay };
                this.SetBinding(IsExpandedProperty, b);
                _headerRoot.Checked += HeaderRoot_Checked;
                _headerRoot.Unchecked += HeaderRoot_Unchecked;
            }

            UpdateVisualState();
        }

        private void HeaderRoot_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Collapsed != null)
            {
                Collapsed.Invoke(this, new EventArgs());
            }
        }

        private void HeaderRoot_Checked(object sender, RoutedEventArgs e)
        {
            if (Expanded != null)
            {
                Expanded.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Updates the visual state based on changes to ExpandDirection and IsExpanded
        /// </summary>
        private void UpdateVisualState()
        {
            switch (ExpandDirection)
            {
                case ExpandDirection.Down:
                    {
                        VisualStateManager.GoToState(this, this.IsExpanded ? VSEXPANDEDDOWN : VSCOLLAPSEDDOWN, true);
                        (GetTemplateChild(ANIMATEINCONTENTDOWN) as Storyboard)?.Begin();
                        break;
                    }

                case ExpandDirection.Up:
                    {
                        VisualStateManager.GoToState(this, this.IsExpanded ? VSEXPANDEDUP : VSCOLLAPSEDUP, true);
                        (GetTemplateChild(ANIMATEINCONTENTUP) as Storyboard)?.Begin();
                        break;
                    }

                case ExpandDirection.Right:
                    {
                        VisualStateManager.GoToState(this, this.IsExpanded ? VSEXPANDEDRIGHT : VSCOLLAPSEDRIGHT, true);
                        (GetTemplateChild(ANIMATEINCONTENTRIGHT) as Storyboard)?.Begin();
                        break;
                    }

                case ExpandDirection.Left:
                    {
                        VisualStateManager.GoToState(this, this.IsExpanded ? VSEXPANDEDLEFT : VSCOLLAPSEDLEFT, true);
                        (GetTemplateChild(ANIMATEINCONTENTLEFT) as Storyboard)?.Begin();
                        break;
                    }
            }
        }
    }
}
