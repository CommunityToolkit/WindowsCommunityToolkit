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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class FocusTracker
    {
        private readonly DispatcherTimer updateTimer;

        public FocusTracker()
        {
            this.InitializeComponent();

            updateTimer = new DispatcherTimer();
            updateTimer.Tick += UpdateTimer_Tick;
        }

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
                ControlFirstParentWithName.Text = string.Empty;
                return;
            }

            ControlName.Text = focusedControl.Name;
            ControlType.Text = focusedControl.GetType().Name;

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
