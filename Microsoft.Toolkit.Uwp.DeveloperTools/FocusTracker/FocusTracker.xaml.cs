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
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.DeveloperTools
{
    /// <summary>
    /// The FocusTracker is used as to display information about the current focused element (if any).
    /// </summary>
    public sealed partial class FocusTracker
    {
        private readonly DispatcherTimer updateTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusTracker"/> class.
        /// </summary>
        public FocusTracker()
        {
            this.InitializeComponent();

            updateTimer = new DispatcherTimer();
            updateTimer.Tick += UpdateTimer_Tick;
        }

        /// <summary>
        /// Gets or sets a boolean indicating whehter the tracker is running or not.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return updateTimer.IsEnabled;
            }

            set
            {
                if (value)
                {
                    updateTimer.Start();
                }
                else
                {
                    updateTimer.Stop();
                }
            }
        }

        private void UpdateTimer_Tick(object sender, object e)
        {
            var focusedControl = FocusManager.GetFocusedElement() as FrameworkElement;

            if (focusedControl == null)
            {
                ControlName.Text = string.Empty;
                ControlType.Text = string.Empty;
                ControlAutomationName.Text = string.Empty;
                ControlFirstParentWithName.Text = string.Empty;
                return;
            }

            ControlName.Text = focusedControl.Name;
            ControlType.Text = focusedControl.GetType().Name;
            ControlAutomationName.Text = AutomationProperties.GetName(focusedControl);

            var parentWithName = FindVisualAscendantWithName(focusedControl);

            ControlFirstParentWithName.Text = parentWithName?.Name ?? string.Empty;
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
