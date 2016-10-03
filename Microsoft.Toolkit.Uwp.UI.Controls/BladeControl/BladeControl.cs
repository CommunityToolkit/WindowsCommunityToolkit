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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="Blade"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeControl : Control
    {
        private ScrollViewer _scrollViewer;

        private static void ToggleBlade(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            Button pressedButton = sender as Button;
            string bladeName = GetToggleBlade(pressedButton);
            BladeControl container = pressedButton.FindVisualAscendant<BladeControl>();
            var blade = container.Blades.FirstOrDefault(_ => _.BladeId == bladeName);

            if (blade == null)
            {
                throw new KeyNotFoundException($"Could not find a blade with ID {bladeName}");
            }

            if (blade.IsOpen)
            {
                blade.IsOpen = false;
                BladeClosed?.Invoke(container, blade);
            }
            else
            {
                blade.IsOpen = true;
                BladeOpened?.Invoke(container, blade);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BladeControl"/> class.
        /// </summary>
        public BladeControl()
        {
            DefaultStyleKey = typeof(BladeControl);
            Blades = new ObservableCollection<Blade>();
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CycleBlades();
        }

        private void CycleBlades()
        {
            ActiveBlades = new ObservableCollection<Blade>();
            foreach (var blade in Blades)
            {
                if (blade.IsOpen)
                {
                    ActiveBlades.Add(blade);
                }

                blade.VisibilityChanged += BladeOnVisibilityChanged;
            }
        }

        private void BladeOnVisibilityChanged(object sender, Visibility visibility)
        {
            var blade = sender as Blade;

            if (visibility == Visibility.Visible)
            {
                ActiveBlades.Add(blade);
                UpdateLayout();
                GetScrollViewer();
                _scrollViewer.ChangeView(_scrollViewer.ScrollableWidth, null, null);
                return;
            }

            ActiveBlades.Remove(blade);
        }

        private void GetScrollViewer()
        {
            if (_scrollViewer == null)
            {
                _scrollViewer = this.FindDescendant<ScrollViewer>();
            }
        }
    }
}
