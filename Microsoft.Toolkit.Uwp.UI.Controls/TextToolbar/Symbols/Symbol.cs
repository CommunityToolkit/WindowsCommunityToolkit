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

using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols
{
    /// <summary>
    /// Common implementation for a symbol icon
    /// </summary>
    [TemplateVisualState(GroupName = Common, Name = Normal)]
    [TemplateVisualState(GroupName = Common, Name = Disabled)]
    public abstract class Symbol : Control
    {
        internal const string Common = "CommonStates";
        internal const string Normal = "Normal";
        internal const string Disabled = "Disabled";

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            isEnabledWatcher = new DependencyPropertyWatcher<bool>(this, nameof(IsEnabled));
            isEnabledWatcher.PropertyChanged += IsEnabledWatcher_PropertyChanged;
            CheckIsEnabled();
            base.OnApplyTemplate();
        }

        private void IsEnabledWatcher_PropertyChanged(object sender, System.EventArgs e)
        {
            CheckIsEnabled();
        }

        private void CheckIsEnabled()
        {
            if (IsEnabled)
            {
                VisualStateManager.GoToState(this, Normal, true);
            }
            else
            {
                VisualStateManager.GoToState(this, Disabled, true);
            }
        }

        private DependencyPropertyWatcher<bool> isEnabledWatcher;

        /// <summary>
        /// Finalizes an instance of the <see cref="Symbol"/> class.
        /// </summary>
        ~Symbol()
        {
            try
            {
                Dispatcher?.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        isEnabledWatcher?.Dispose();
                    }
                    catch (System.Exception)
                    {
                    }
                });
            }
            catch (System.Exception)
            {
            }
        }
    }
}