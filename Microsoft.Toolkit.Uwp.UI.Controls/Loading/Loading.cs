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
    [TemplatePart(Name = "RootGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "BackgroundGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "ContentGrid", Type = typeof(ContentPresenter))]
    public partial class Loading : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Loading"/> class.
        /// </summary>
        public Loading()
        {
            DefaultStyleKey = typeof(Loading);
        }

        protected override void OnApplyTemplate()
        {
            _rootGrid = GetTemplateChild("RootGrid") as Grid;
            _backgroundGrid = GetTemplateChild("BackgroundGrid") as Grid;
            _contentGrid = GetTemplateChild("ContentGrid") as ContentPresenter;

            OnLoadingRequired();

            CreateLoadingControl();

            base.OnApplyTemplate();
        }

        protected virtual void OnLoadingRequired()
        {
            LoadingRequired?.Invoke(this, EventArgs.Empty);
        }

        private void CreateLoadingControl()
        {
            if (IsLoading)
            {
                if (LoadingBackground == null && LoadingOpacity == 0d)
                {
                    _backgroundGrid = null;
                }
                else
                {
                    _backgroundGrid.Background = LoadingBackground;
                    _backgroundGrid.Opacity = LoadingOpacity;
                }

                VisualStateManager.GoToState(this, "LoadingIn", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "LoadingOut", true);
            }
        }
    }
}
