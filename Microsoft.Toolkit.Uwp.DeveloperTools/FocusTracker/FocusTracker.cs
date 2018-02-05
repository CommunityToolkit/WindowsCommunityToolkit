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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.DeveloperTools
{
    /// <summary>
    /// FocusTracker can be used to display information about the current focused XAML element.
    /// </summary>
    [TemplatePart(Name = "ControlName", Type = typeof(TextBlock))]
    [TemplatePart(Name = "ControlType", Type = typeof(TextBlock))]
    [TemplatePart(Name = "ControlAutomationName", Type = typeof(TextBlock))]
    [TemplatePart(Name = "ControlFirstParentWithName", Type = typeof(TextBlock))]
    public class FocusTracker : Control
    {
        /// <summary>
        /// Defines the <see cref="IsActive"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(FocusTracker), new PropertyMetadata(false, OnIsActiveChanged));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var focusTracker = d as FocusTracker;

            if (e.NewValue != null && (bool)e.NewValue)
            {
                focusTracker?.Start();
            }
            else
            {
                focusTracker?.Stop();
            }
        }

        private DispatcherTimer updateTimer;
        private TextBlock controlName;
        private TextBlock controlType;
        private TextBlock controlAutomationName;
        private TextBlock controlFirstParentWithName;

        /// <summary>
        /// Gets or sets a boolean indicating whether the tracker is running or not.
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusTracker"/> class.
        /// </summary>
        public FocusTracker()
        {
            DefaultStyleKey = typeof(FocusTracker);
        }

        private void Start()
        {
            if (updateTimer == null)
            {
                updateTimer = new DispatcherTimer();
                updateTimer.Tick += UpdateTimer_Tick;
            }

            updateTimer.Start();
        }

        private void Stop()
        {
            updateTimer?.Stop();
            ClearContent();
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            controlName = GetTemplateChild("ControlName") as TextBlock;
            controlType = GetTemplateChild("ControlType") as TextBlock;
            controlAutomationName = GetTemplateChild("ControlAutomationName") as TextBlock;
            controlFirstParentWithName = GetTemplateChild("ControlFirstParentWithName") as TextBlock;
        }

        private void ClearContent()
        {
            controlName.Text = string.Empty;
            controlType.Text = string.Empty;
            controlAutomationName.Text = string.Empty;
            controlFirstParentWithName.Text = string.Empty;
        }

        private void UpdateTimer_Tick(object sender, object e)
        {
            var focusedControl = FocusManager.GetFocusedElement() as FrameworkElement;

            if (focusedControl == null)
            {
                ClearContent();
                return;
            }

            if (controlName != null)
            {
                controlName.Text = focusedControl.Name;
            }

            if (controlType != null)
            {
                controlType.Text = focusedControl.GetType().Name;
            }

            if (controlAutomationName != null)
            {
                controlAutomationName.Text = AutomationProperties.GetName(focusedControl);
            }

            if (controlFirstParentWithName != null)
            {
                var parentWithName = FindVisualAscendantWithName(focusedControl);
                controlFirstParentWithName.Text = parentWithName?.Name ?? string.Empty;
            }
        }

        private FrameworkElement FindVisualAscendantWithName(FrameworkElement element)
        {
            var parent = VisualTreeHelper.GetParent(element) as FrameworkElement;

            if (parent == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(parent.Name))
            {
                return parent;
            }

            return FindVisualAscendantWithName(parent);
        }
    }
}
