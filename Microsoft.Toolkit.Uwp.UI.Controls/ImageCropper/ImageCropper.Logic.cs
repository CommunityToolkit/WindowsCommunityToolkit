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
    /// The <see cref="ImageCropper"/> control allows user to crop image freely.
    /// </summary>
    public partial class ImageCropper
    {
        /// <summary>
        /// Initializes image source transform.
        /// </summary>
        private void InitImageLayout()
        {
            _restrictedCropRect = new Rect(0, 0, Source.PixelWidth, Source.PixelHeight);
            var maxSelectedRect = _restrictedCropRect;
            _currentCroppedRect = KeepAspectRatio ? GetUniformRect(maxSelectedRect, UsedAspectRatio) : maxSelectedRect;
            UpdateImageLayout();
            UpdateThumbsVisibility();
        }

        /// <summary>
        /// Update image source transform.
        /// </summary>
        private void UpdateImageLayout()
        {
            var uniformSelectedRect = GetUniformRect(CanvasRect, _currentCroppedRect.Width / _currentCroppedRect.Height);
            UpdateImageLayoutWithViewport(uniformSelectedRect, _currentCroppedRect);
        }

        /// <summary>
        /// Update image source transform.
        /// </summary>
        /// <param name="viewport">Viewport</param>
        /// <param name="viewportImageRect"> The real image area of viewport.</param>
        private void UpdateImageLayoutWithViewport(Rect viewport, Rect viewportImageRect)
        {
            var imageScale = viewport.Width / viewportImageRect.Width;
            _imageTransform.ScaleX = _imageTransform.ScaleY = imageScale;
            _imageTransform.TranslateX = viewport.X - (viewportImageRect.X * imageScale);
            _imageTransform.TranslateY = viewport.Y - (viewportImageRect.Y * imageScale);
            var selectedRect = _imageTransform.TransformBounds(_currentCroppedRect);
            _restrictedSelectRect = _imageTransform.TransformBounds(_restrictedCropRect);
            var startPoint = GetSafePoint(_restrictedSelectRect, new Point(selectedRect.X, selectedRect.Y));
            var endPoint = GetSafePoint(_restrictedSelectRect, new Point(
                selectedRect.X + selectedRect.Width,
                selectedRect.Y + selectedRect.Height));
            UpdateSelectedRect(startPoint, endPoint);
        }

        /// <summary>
        /// Update cropped area.
        /// </summary>
        /// <param name="position">The control point</param>
        /// <param name="diffPos">Position offset</param>
        private void UpdateCroppedRectWithAspectRatio(ThumbPosition position, Point diffPos)
        {
            double radian = 0d, diffPointRadian = 0d, effectiveLength = 0d;
            if (KeepAspectRatio)
            {
                radian = Math.Atan(UsedAspectRatio);
                diffPointRadian = Math.Atan(diffPos.X / diffPos.Y);
            }

            var startPoint = new Point(_startX, _startY);
            var endPoint = new Point(_endX, _endY);
            switch (position)
            {
                case ThumbPosition.Top:
                    startPoint.Y += diffPos.Y;
                    if (KeepAspectRatio)
                    {
                        var changeX = diffPos.Y * UsedAspectRatio;
                        startPoint.X += changeX / 2;
                        endPoint.X -= changeX / 2;
                    }

                    break;
                case ThumbPosition.Bottom:
                    endPoint.Y += diffPos.Y;
                    if (KeepAspectRatio)
                    {
                        var changeX = diffPos.Y * UsedAspectRatio;
                        startPoint.X -= changeX / 2;
                        endPoint.X += changeX / 2;
                    }

                    break;
                case ThumbPosition.Left:
                    startPoint.X += diffPos.X;
                    if (KeepAspectRatio)
                    {
                        var changeY = diffPos.X / UsedAspectRatio;
                        startPoint.Y += changeY / 2;
                        endPoint.Y -= changeY / 2;
                    }

                    break;
                case ThumbPosition.Right:
                    endPoint.X += diffPos.X;
                    if (KeepAspectRatio)
                    {
                        var changeY = diffPos.X / UsedAspectRatio;
                        startPoint.Y -= changeY / 2;
                        endPoint.Y += changeY / 2;
                    }

                    break;
                case ThumbPosition.UpperLeft:
                    if (KeepAspectRatio)
                    {
                        effectiveLength = diffPos.Y / Math.Cos(diffPointRadian) * Math.Cos(diffPointRadian - radian);
                        diffPos.X = effectiveLength * Math.Sin(radian);
                        diffPos.Y = effectiveLength * Math.Cos(radian);
                    }

                    startPoint.X += diffPos.X;
                    startPoint.Y += diffPos.Y;
                    break;
                case ThumbPosition.UpperRight:
                    if (KeepAspectRatio)
                    {
                        diffPointRadian = -diffPointRadian;
                        effectiveLength = diffPos.Y / Math.Cos(diffPointRadian) * Math.Cos(diffPointRadian - radian);
                        diffPos.X = -effectiveLength * Math.Sin(radian);
                        diffPos.Y = effectiveLength * Math.Cos(radian);
                    }

                    endPoint.X += diffPos.X;
                    startPoint.Y += diffPos.Y;
                    break;
                case ThumbPosition.LowerLeft:
                    if (KeepAspectRatio)
                    {
                        diffPointRadian = -diffPointRadian;
                        effectiveLength = diffPos.Y / Math.Cos(diffPointRadian) * Math.Cos(diffPointRadian - radian);
                        diffPos.X = -effectiveLength * Math.Sin(radian);
                        diffPos.Y = effectiveLength * Math.Cos(radian);
                    }

                    startPoint.X += diffPos.X;
                    endPoint.Y += diffPos.Y;
                    break;
                case ThumbPosition.LowerRight:
                    if (KeepAspectRatio)
                    {
                        effectiveLength = diffPos.Y / Math.Cos(diffPointRadian) * Math.Cos(diffPointRadian - radian);
                        diffPos.X = effectiveLength * Math.Sin(radian);
                        diffPos.Y = effectiveLength * Math.Cos(radian);
                    }

                    endPoint.X += diffPos.X;
                    endPoint.Y += diffPos.Y;
                    break;
            }

            if (!IsSafeRect(startPoint, endPoint, MinSelectSize))
            {
                if (KeepAspectRatio)
                {
                    if ((endPoint.Y - startPoint.Y) < (_endY - _startY) ||
                        (endPoint.X - startPoint.X) < (_endX - _startX))
                    {
                        return;
                    }
                }
                else
                {
                    var safeRect = GetSafeRect(startPoint, endPoint, MinSelectSize, position);
                    safeRect.Intersect(_restrictedSelectRect);
                    startPoint = new Point(safeRect.X, safeRect.Y);
                    endPoint = new Point(safeRect.X + safeRect.Width, safeRect.Y + safeRect.Height);
                }
            }

            var isEffectiveRegion = IsSafePoint(_restrictedSelectRect, startPoint) &&
                                    IsSafePoint(_restrictedSelectRect, endPoint);
            if (!isEffectiveRegion)
            {
                return;
            }

            var selectedRect = new Rect(startPoint, endPoint);
            selectedRect.Union(CanvasRect);
            if (selectedRect != CanvasRect)
            {
                var inverseImageTransform = _imageTransform.Inverse;
                if (inverseImageTransform == null)
                {
                    return;
                }

                var croppedRect = inverseImageTransform.TransformBounds(
                    new Rect(startPoint, endPoint));
                croppedRect.Intersect(_restrictedCropRect);
                _currentCroppedRect = croppedRect;
                var viewportRect = GetUniformRect(CanvasRect, selectedRect.Width / selectedRect.Height);
                var viewportImgRect = inverseImageTransform.TransformBounds(selectedRect);
                UpdateImageLayoutWithViewport(viewportRect, viewportImgRect);
            }
            else
            {
                UpdateSelectedRect(startPoint, endPoint);
            }
        }

        /// <summary>
        /// Update selection area.
        /// </summary>
        /// <param name="startPoint">The point on the upper left corner.</param>
        /// <param name="endPoint">The point on the lower right corner.</param>
        private void UpdateSelectedRect(Point startPoint, Point endPoint)
        {
            _startX = startPoint.X;
            _startY = startPoint.Y;
            _endX = endPoint.X;
            _endY = endPoint.Y;
            var centerX = ((_endX - _startX) / 2) + _startX;
            var centerY = ((_endY - _startY) / 2) + _startY;
            if (_topThumb != null)
            {
                _topThumb.X = centerX;
                _topThumb.Y = _startY;
            }

            if (_bottomThumb != null)
            {
                _bottomThumb.X = centerX;
                _bottomThumb.Y = _endY;
            }

            if (_leftThumb != null)
            {
                _leftThumb.X = _startX;
                _leftThumb.Y = centerY;
            }

            if (_rightThumb != null)
            {
                _rightThumb.X = _endX;
                _rightThumb.Y = centerY;
            }

            if (_upperLeftThumb != null)
            {
                _upperLeftThumb.X = _startX;
                _upperLeftThumb.Y = _startY;
            }

            if (_upperRightThumb != null)
            {
                _upperRightThumb.X = _endX;
                _upperRightThumb.Y = _startY;
            }

            if (_lowerLeftThumb != null)
            {
                _lowerLeftThumb.X = _startX;
                _lowerLeftThumb.Y = _endY;
            }

            if (_lowerRigthThumb != null)
            {
                _lowerRigthThumb.X = _endX;
                _lowerRigthThumb.Y = _endY;
            }

            UpdateMaskArea();
        }

        /// <summary>
        /// Update the mask layer.
        /// </summary>
        private void UpdateMaskArea()
        {
            if (_layoutGrid == null)
            {
                return;
            }

            _maskAreaGeometryGroup.Children.Clear();
            _maskAreaGeometryGroup.Children.Add(new RectangleGeometry
            {
                Rect = new Rect(-_layoutGrid.Padding.Left, -_layoutGrid.Padding.Top, _layoutGrid.ActualWidth, _layoutGrid.ActualHeight)
            });

            switch (CropShape)
            {
                case CropShape.Rectangular:
                    _maskAreaGeometryGroup.Children.Add(new RectangleGeometry
                    {
                        Rect = new Rect(new Point(_startX, _startY), new Point(_endX, _endY))
                    });
                    break;
                case CropShape.Circular:
                    var centerX = ((_endX - _startX) / 2) + _startX;
                    var centerY = ((_endY - _startY) / 2) + _startY;
                    _maskAreaGeometryGroup.Children.Add(new EllipseGeometry
                    {
                        Center = new Point(centerX, centerY),
                        RadiusX = (_endX - _startX) / 2,
                        RadiusY = (_endY - _startY) / 2
                    });
                    break;
            }

            _layoutGrid.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, _layoutGrid.ActualWidth, _layoutGrid.ActualHeight)
            };
        }

        /// <summary>
        /// Update image aspect ratio.
        /// </summary>
        private void UpdateAspectRatio()
        {
            if (KeepAspectRatio && Source != null)
            {
                var inverseImageTransform = _imageTransform.Inverse;
                if (inverseImageTransform != null)
                {
                    var centerX = ((_endX - _startX) / 2) + _startX;
                    var centerY = ((_endY - _startY) / 2) + _startY;
                    var restrictedMinLength = MinCroppedPixelLength * _imageTransform.ScaleX;
                    var maxSelectedLength = Math.Max(_endX - _startX, _endY - _startY);
                    var viewRect = new Rect(centerX - (maxSelectedLength / 2), centerY - (maxSelectedLength / 2), maxSelectedLength, maxSelectedLength);
                    var uniformSelectedRect = GetUniformRect(viewRect, UsedAspectRatio);
                    if (uniformSelectedRect.Width > _restrictedSelectRect.Width || uniformSelectedRect.Height > _restrictedSelectRect.Height)
                    {
                        uniformSelectedRect = GetUniformRect(_restrictedSelectRect, UsedAspectRatio);
                    }

                    if (uniformSelectedRect.Width < restrictedMinLength || uniformSelectedRect.Height < restrictedMinLength)
                    {
                        var scale = restrictedMinLength / Math.Min(uniformSelectedRect.Width, uniformSelectedRect.Height);
                        uniformSelectedRect.Width *= scale;
                        uniformSelectedRect.Height *= scale;
                        if (uniformSelectedRect.Width > _restrictedSelectRect.Width || uniformSelectedRect.Height > _restrictedSelectRect.Height)
                        {
                            AspectRatio = -1;
                            return;
                        }
                    }

                    if (_restrictedSelectRect.X > uniformSelectedRect.X)
                    {
                        uniformSelectedRect.X += _restrictedSelectRect.X - uniformSelectedRect.X;
                    }

                    if (_restrictedSelectRect.Y > uniformSelectedRect.Y)
                    {
                        uniformSelectedRect.Y += _restrictedSelectRect.Y - uniformSelectedRect.Y;
                    }

                    if ((_restrictedSelectRect.X + _restrictedSelectRect.Width) < (uniformSelectedRect.X + uniformSelectedRect.Width))
                    {
                        uniformSelectedRect.X += (_restrictedSelectRect.X + _restrictedSelectRect.Width) - (uniformSelectedRect.X + uniformSelectedRect.Width);
                    }

                    if ((_restrictedSelectRect.Y + _restrictedSelectRect.Height) < (uniformSelectedRect.Y + uniformSelectedRect.Height))
                    {
                        uniformSelectedRect.Y += (_restrictedSelectRect.Y + _restrictedSelectRect.Height) - (uniformSelectedRect.Y + uniformSelectedRect.Height);
                    }

                    var croppedRect = inverseImageTransform.TransformBounds(uniformSelectedRect);
                    croppedRect.Intersect(_restrictedCropRect);
                    _currentCroppedRect = croppedRect;
                    UpdateImageLayout();
                }
            }
        }

        /// <summary>
        /// Update the visibility of the thumbs.
        /// </summary>
        private void UpdateThumbsVisibility()
        {
            var cornerThumbsVisibility = Visibility.Visible;
            var otherThumbsVisibility = Visibility.Visible;
            switch (ThumbPlacement)
            {
                case ThumbPlacement.All:
                    break;
                case ThumbPlacement.Corners:
                    otherThumbsVisibility = Visibility.Collapsed;
                    break;
            }

            switch (CropShape)
            {
                case CropShape.Rectangular:
                    break;
                case CropShape.Circular:
                    cornerThumbsVisibility = Visibility.Collapsed;
                    otherThumbsVisibility = Visibility.Visible;
                    break;
            }

            if (Source == null)
            {
                cornerThumbsVisibility = otherThumbsVisibility = Visibility.Collapsed;
            }

            if (_topThumb != null)
            {
                _topThumb.Visibility = otherThumbsVisibility;
            }

            if (_bottomThumb != null)
            {
                _bottomThumb.Visibility = otherThumbsVisibility;
            }

            if (_leftThumb != null)
            {
                _leftThumb.Visibility = otherThumbsVisibility;
            }

            if (_rightThumb != null)
            {
                _rightThumb.Visibility = otherThumbsVisibility;
            }

            if (_upperLeftThumb != null)
            {
                _upperLeftThumb.Visibility = cornerThumbsVisibility;
            }

            if (_upperRightThumb != null)
            {
                _upperRightThumb.Visibility = cornerThumbsVisibility;
            }

            if (_lowerLeftThumb != null)
            {
                _lowerLeftThumb.Visibility = cornerThumbsVisibility;
            }

            if (_lowerRigthThumb != null)
            {
                _lowerRigthThumb.Visibility = cornerThumbsVisibility;
            }
        }
    }
}
