// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base Code for ImageEx
    /// </summary>
    [TemplateVisualState(Name = LoadingState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = LoadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = UnloadedState, GroupName = CommonGroup)]
    [TemplateVisualState(Name = FailedState, GroupName = CommonGroup)]
    [TemplatePart(Name = PartImage, Type = typeof(object))]
    [TemplatePart(Name = PartProgress, Type = typeof(ProgressRing))]
    public abstract partial class ImageExBase : Control
    {
        private bool _isInViewport;

        /// <summary>
        /// Image name in template
        /// </summary>
        protected const string PartImage = "Image";

        /// <summary>
        /// ProgressRing name in template
        /// </summary>
        protected const string PartProgress = "Progress";

        /// <summary>
        /// VisualStates name in template
        /// </summary>
        protected const string CommonGroup = "CommonStates";

        /// <summary>
        /// Loading state name in template
        /// </summary>
        protected const string LoadingState = "Loading";

        /// <summary>
        /// Loaded state name in template
        /// </summary>
        protected const string LoadedState = "Loaded";

        /// <summary>
        /// Unloaded state name in template
        /// </summary>
        protected const string UnloadedState = "Unloaded";

        /// <summary>
        /// Failed name in template
        /// </summary>
        protected const string FailedState = "Failed";

        /// <summary>
        /// Gets the backing image object
        /// </summary>
        protected object Image { get; private set; }

        /// <summary>
        /// Gets backing object for the ProgressRing
        /// </summary>
        protected ProgressRing Progress { get; private set; }

        /// <summary>
        /// Gets object used for lock
        /// </summary>
        protected object LockObj { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExBase"/> class.
        /// </summary>
        public ImageExBase()
        {
            LockObj = new object();

            if (IsLazyLoadingSupported)
            {
                LayoutUpdated += ImageExBase_LayoutUpdated;
            }
        }

        /// <summary>
        /// Attach image opened event handler
        /// </summary>
        /// <param name="handler">Routed Event Handler</param>
        protected void AttachImageOpened(RoutedEventHandler handler)
        {
            var image = Image as Image;
            var brush = Image as ImageBrush;

            if (image != null)
            {
                image.ImageOpened += handler;
            }
            else if (brush != null)
            {
                brush.ImageOpened += handler;
            }
        }

        /// <summary>
        /// Remove image opened handler
        /// </summary>
        /// <param name="handler">RoutedEventHandler</param>
        protected void RemoveImageOpened(RoutedEventHandler handler)
        {
            var image = Image as Image;
            var brush = Image as ImageBrush;

            if (image != null)
            {
                image.ImageOpened -= handler;
            }
            else if (brush != null)
            {
                brush.ImageOpened -= handler;
            }
        }

        /// <summary>
        /// Attach image failed event handler
        /// </summary>
        /// <param name="handler">Exception Routed Event Handler</param>
        protected void AttachImageFailed(ExceptionRoutedEventHandler handler)
        {
            var image = Image as Image;
            var brush = Image as ImageBrush;

            if (image != null)
            {
                image.ImageFailed += handler;
            }
            else if (brush != null)
            {
                brush.ImageFailed += handler;
            }
        }

        /// <summary>
        /// Remove Image Failed handler
        /// </summary>
        /// <param name="handler">Exception Routed Event Handler</param>
        protected void RemoveImageFailed(ExceptionRoutedEventHandler handler)
        {
            var image = Image as Image;
            var brush = Image as ImageBrush;

            if (image != null)
            {
                image.ImageFailed -= handler;
            }
            else if (brush != null)
            {
                brush.ImageFailed -= handler;
            }
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            RemoveImageOpened(OnImageOpened);
            RemoveImageFailed(OnImageFailed);

            Image = GetTemplateChild(PartImage) as object;
            Progress = GetTemplateChild(PartProgress) as ProgressRing;

            IsInitialized = true;

            ImageExInitialized?.Invoke(this, EventArgs.Empty);

            if (IsLazyLoadingSupported)
            {
                if (Source == null || !EnableLazyLoading || _isInViewport)
                {
                    _lazyLoadingSource = null;
                    SetSource(Source);
                }
                else
                {
                    _lazyLoadingSource = Source;
                }
            }
            else
            {
                SetSource(Source);
            }

            AttachImageOpened(OnImageOpened);
            AttachImageFailed(OnImageFailed);

            base.OnApplyTemplate();
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var newSquareSize = Math.Min(finalSize.Width, finalSize.Height) / 8.0;

            if (Progress?.Width == newSquareSize)
            {
                Progress.Height = newSquareSize;
            }

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// from https://referencesource.microsoft.com/#WindowsBase/Base/System/Windows/Rect.cs,04390f584fcba8e4
        /// </summary>
        private static bool IntersectsWith(Rect rect1, Rect rect2)
        {
            if (rect1.IsEmpty || rect2.IsEmpty)
            {
                return false;
            }

            return (rect1.Left <= rect2.Right) &&
                   (rect1.Right >= rect2.Left) &&
                   (rect1.Top <= rect2.Bottom) &&
                   (rect1.Bottom >= rect2.Top);
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            ImageExOpened?.Invoke(this, new ImageExOpenedEventArgs());
            VisualStateManager.GoToState(this, LoadedState, true);
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageExFailed?.Invoke(this, new ImageExFailedEventArgs(new Exception(e.ErrorMessage)));
            VisualStateManager.GoToState(this, FailedState, true);
        }

        private void ImageExBase_LayoutUpdated(object sender, object e)
        {
            InvalidateLazyLoading();
        }

        private void InvalidateLazyLoading()
        {
            if (!IsLoaded)
            {
                _isInViewport = false;
                return;
            }

            var hostElement = GetHostElement();
            if (hostElement == null)
            {
                _isInViewport = false;
                return;
            }

            var controlRect = TransformToVisual(hostElement)
                .TransformBounds(new Rect(0, 0, ActualWidth, ActualHeight));
            var lazyLoadingThreshold = LazyLoadingThreshold;
            var hostRect = new Rect(
                0 - lazyLoadingThreshold,
                0 - lazyLoadingThreshold,
                hostElement.ActualWidth + (2 * lazyLoadingThreshold),
                hostElement.ActualHeight + (2 * lazyLoadingThreshold));

            if (IntersectsWith(controlRect, hostRect))
            {
                _isInViewport = true;

                if (_lazyLoadingSource != null)
                {
                    var source = _lazyLoadingSource;
                    _lazyLoadingSource = null;
                    SetSource(source);
                }
            }
            else
            {
                _isInViewport = false;
            }
        }

        /// <summary>
        /// Find ascendant element until <see cref="ScrollViewer" /> or root element.
        /// </summary>
        /// <returns><see cref="ScrollViewer"/> or root element.</returns>
        private FrameworkElement GetHostElement()
        {
            FrameworkElement hostElement = this;
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(hostElement) as FrameworkElement;
                if (parent == null)
                {
                    break;
                }

                if (parent is ScrollViewer)
                {
                    return parent;
                }

                hostElement = parent;
            }

            if (ReferenceEquals(hostElement, this))
            {
                return null;
            }

            return hostElement;
        }
    }
}