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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Shared Code for ImageEx and RoundImageEx
    /// </summary>
    [TemplateVisualState(Name = LoadingState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = LoadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = UnloadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = FailedState, GroupName = CommonGroup)]
    [TemplatePart(Name = PartImage, Type = typeof(object))]
    [TemplatePart(Name = PartProgress, Type = typeof(ProgressRing))]
    public abstract partial class ImageExBase : Control
    {
        protected const string PartImage = "Image";
        protected const string PartProgress = "Progress";
        protected const string CommonGroup = "CommonStates";
        protected const string LoadingState = "Loading";
        protected const string LoadedState = "Loaded";
        protected const string UnloadedState = "Unloaded";
        protected const string FailedState = "Failed";

        private object _image;
        private ProgressRing _progress;
        private bool _isInitialized;
        private object _lockObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExBase"/> class.
        /// </summary>
        public ImageExBase()
        {
            _lockObj = new object();
        }

        protected void AttachImageOpened(RoutedEventHandler handler)
        {
            dynamic image = _image;
            if (image != null)
            {
                image.ImageOpened += handler;
            }
        }

        protected void RemoveImageOpened(RoutedEventHandler handler)
        {
            dynamic image = _image;
            if (image != null)
            {
                image.ImageOpened -= handler;
            }
        }

        protected void AttachImageFailed(ExceptionRoutedEventHandler handler)
        {
            dynamic image = _image;
            if (image != null)
            {
                image.ImageFailed += handler;
            }
        }

        protected void RemoveImageFailed(ExceptionRoutedEventHandler handler)
        {
            dynamic image = _image;
            if (image != null)
            {
                image.ImageFailed -= handler;
            }
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            RemoveImageOpened(OnImageOpened);
            RemoveImageFailed(OnImageFailed);

            _image = GetTemplateChild(PartImage) as object;
            _progress = GetTemplateChild(PartProgress) as ProgressRing;

            _isInitialized = true;

            SetSource(Source);

            AttachImageOpened(OnImageOpened);
            AttachImageFailed(OnImageFailed);

            base.OnApplyTemplate();
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var newSquareSize = Math.Min(finalSize.Width, finalSize.Height) / 8.0;

            if (_progress?.Width == newSquareSize)
            {
                _progress.Height = newSquareSize;
            }

            return base.ArrangeOverride(finalSize);
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            ImageOpened?.Invoke(this, e);
            ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
            VisualStateManager.GoToState(this, LoadedState, true);
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageFailed?.Invoke(this, e);
            ImageExFailed?.Invoke(this, new ImageExFailedEventArgs(new Exception(e.ErrorMessage)));
            VisualStateManager.GoToState(this, FailedState, true);
        }
    }