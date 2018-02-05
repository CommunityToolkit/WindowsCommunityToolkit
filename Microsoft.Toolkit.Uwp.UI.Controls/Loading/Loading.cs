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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Loading control allows to show an loading animation with some xaml in it.
    /// </summary>
    [TemplateVisualState(Name = "LoadingIn", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "LoadingOut", GroupName = "CommonStates")]
    public partial class Loading : ContentControl
    {
        private FrameworkElement _presenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Loading"/> class.
        /// </summary>
        public Loading()
        {
            DefaultStyleKey = typeof(Loading);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Update();
        }

        private void Update()
        {
            VisualStateManager.GoToState(this, IsLoading ? "LoadingIn" : "LoadingOut", true);
        }
    }
}
