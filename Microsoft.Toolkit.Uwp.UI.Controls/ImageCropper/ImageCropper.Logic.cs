using ImageCropper.UWP.Extensions;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ImageCropper.UWP
{
    public partial class ImageCropper
    {
        /// <summary>
        /// Initializes image source transform.
        /// </summary>
        private void InitImageLayout()
        {
            _restrictedCropRect = new Rect(0, 0, SourceImage.PixelWidth, SourceImage.PixelHeight);
            var maxSelectedRect = _restrictedCropRect;
            _currentCroppedRect = KeepAspectRatio ? maxSelectedRect.GetUniformRect(UsedAspectRatio) : maxSelectedRect;
            UpdateImageLayout();
            UpdateControlButtonVisibility();
        }

        /// <summary>
        /// Update image source transform.
        /// </summary>
        private void UpdateImageLayout()
        {
            var uniformSelectedRect = CanvasRect.GetUniformRect(_currentCroppedRect.Width / _currentCroppedRect.Height);
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
            _imageTransform.TranslateX = viewport.X - viewportImageRect.X * imageScale;
            _imageTransform.TranslateY = viewport.Y - viewportImageRect.Y * imageScale;
            var selectedRect = _imageTransform.TransformBounds(_currentCroppedRect);
            _restrictedSelectRect = _imageTransform.TransformBounds(_restrictedCropRect);
            var startPoint = _restrictedSelectRect.GetSafePoint(new Point(selectedRect.X, selectedRect.Y));
            var endPoint = _restrictedSelectRect.GetSafePoint(new Point(selectedRect.X + selectedRect.Width,
                selectedRect.Y + selectedRect.Height));
            UpdateSelectedRect(startPoint, endPoint);
        }

        /// <summary>
        /// Update cropped area.
        /// </summary>
        /// <param name="dragPosition">The control point</param>
        /// <param name="diffPos">Position offset</param>
        private void UpdateCroppedRectWithAspectRatio(DragPosition dragPosition, Point diffPos)
        {
            double radian = 0d, diffPointRadian = 0d, effectiveLength = 0d;
            if (KeepAspectRatio)
            {
                radian = Math.Atan(UsedAspectRatio);
                diffPointRadian = Math.Atan(diffPos.X / diffPos.Y);
            }

            var startPoint = new Point(_startX, _startY);
            var endPoint = new Point(_endX, _endY);
            switch (dragPosition)
            {
                case DragPosition.Top:
                    startPoint.Y += diffPos.Y;
                    if (KeepAspectRatio)
                    {
                        var changeX = diffPos.Y * UsedAspectRatio;
                        startPoint.X += changeX / 2;
                        endPoint.X -= changeX / 2;
                    }

                    break;
                case DragPosition.Bottom:
                    endPoint.Y += diffPos.Y;
                    if (KeepAspectRatio)
                    {
                        var changeX = diffPos.Y * UsedAspectRatio;
                        startPoint.X -= changeX / 2;
                        endPoint.X += changeX / 2;
                    }

                    break;
                case DragPosition.Left:
                    startPoint.X += diffPos.X;
                    if (KeepAspectRatio)
                    {
                        var changeY = diffPos.X / UsedAspectRatio;
                        startPoint.Y += changeY / 2;
                        endPoint.Y -= changeY / 2;
                    }

                    break;
                case DragPosition.Right:
                    endPoint.X += diffPos.X;
                    if (KeepAspectRatio)
                    {
                        var changeY = diffPos.X / UsedAspectRatio;
                        startPoint.Y -= changeY / 2;
                        endPoint.Y += changeY / 2;
                    }

                    break;
                case DragPosition.UpperLeft:
                    if (KeepAspectRatio)
                    {
                        effectiveLength = diffPos.Y / Math.Cos(diffPointRadian) * Math.Cos(diffPointRadian - radian);
                        diffPos.X = effectiveLength * Math.Sin(radian);
                        diffPos.Y = effectiveLength * Math.Cos(radian);
                    }

                    startPoint.X += diffPos.X;
                    startPoint.Y += diffPos.Y;
                    break;
                case DragPosition.UpperRight:
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
                case DragPosition.LowerLeft:
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
                case DragPosition.LowerRight:
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

            if (!RectExtensions.IsSafeRect(startPoint, endPoint, MinSelectSize))
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
                    var safeRect = RectExtensions.GetSafeRect(startPoint, endPoint, MinSelectSize, dragPosition);
                    safeRect.Intersect(_restrictedSelectRect);
                    startPoint = new Point(safeRect.X, safeRect.Y);
                    endPoint = new Point(safeRect.X + safeRect.Width, safeRect.Y + safeRect.Height);
                }
            }

            var isEffectiveRegion = _restrictedSelectRect.IsSafePoint(startPoint) &&
                                    _restrictedSelectRect.IsSafePoint(endPoint);
            if (!isEffectiveRegion) return;
            var selectedRect = new Rect(startPoint, endPoint);
            selectedRect.Union(CanvasRect);
            if (selectedRect != CanvasRect)
            {
                var inverseImageTransform = _imageTransform.Inverse;
                if (inverseImageTransform == null) return;
                var croppedRect = inverseImageTransform.TransformBounds(
                    new Rect(startPoint, endPoint));
                croppedRect.Intersect(_restrictedCropRect);
                _currentCroppedRect = croppedRect;
                var viewportRect = CanvasRect.GetUniformRect(selectedRect.Width / selectedRect.Height);
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
            var centerX = (_endX - _startX) / 2 + _startX;
            var centerY = (_endY - _startY) / 2 + _startY;
            if (_topButton != null)
            {
                Canvas.SetLeft(_topButton, centerX);
                Canvas.SetTop(_topButton, _startY);
            }

            if (_bottomButton != null)
            {
                Canvas.SetLeft(_bottomButton, centerX);
                Canvas.SetTop(_bottomButton, _endY);
            }

            if (_leftButton != null)
            {
                Canvas.SetLeft(_leftButton, _startX);
                Canvas.SetTop(_leftButton, centerY);
            }

            if (_rigthButton != null)
            {
                Canvas.SetLeft(_rigthButton, _endX);
                Canvas.SetTop(_rigthButton, centerY);
            }

            if (_upperLeftButton != null)
            {
                Canvas.SetLeft(_upperLeftButton, _startX);
                Canvas.SetTop(_upperLeftButton, _startY);
            }

            if (_upperRightButton != null)
            {
                Canvas.SetLeft(_upperRightButton, _endX);
                Canvas.SetTop(_upperRightButton, _startY);
            }

            if (_lowerLeftButton != null)
            {
                Canvas.SetLeft(_lowerLeftButton, _startX);
                Canvas.SetTop(_lowerLeftButton, _endY);
            }

            if (_lowerRigthButton != null)
            {
                Canvas.SetLeft(_lowerRigthButton, _endX);
                Canvas.SetTop(_lowerRigthButton, _endY);
            }

            UpdateMaskArea();
        }

        /// <summary>
        /// Update the mask layer.
        /// </summary>
        private void UpdateMaskArea()
        {
            _maskAreaGeometryGroup.Children.Clear();
            _maskAreaGeometryGroup.Children.Add(new RectangleGeometry
            {
                Rect = new Rect(-_layoutGrid.Padding.Left, -_layoutGrid.Padding.Top, _layoutGrid.ActualWidth,
                    _layoutGrid.ActualHeight)
            });
            if (CircularCrop)
            {
                var centerX = (_endX - _startX) / 2 + _startX;
                var centerY = (_endY - _startY) / 2 + _startY;
                _maskAreaGeometryGroup.Children.Add(new EllipseGeometry
                {
                    Center = new Point(centerX, centerY),
                    RadiusX = (_endX - _startX) / 2,
                    RadiusY = (_endY - _startY) / 2
                });
            }
            else
            {
                _maskAreaGeometryGroup.Children.Add(new RectangleGeometry
                {
                    Rect = new Rect(new Point(_startX, _startY), new Point(_endX, _endY))
                });
            }

            _layoutGrid.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, _layoutGrid.ActualWidth,
                    _layoutGrid.ActualHeight)
            };
        }

        /// <summary>
        /// Update image aspect ratio.
        /// </summary>
        private void UpdateAspectRatio()
        {
            if (KeepAspectRatio && SourceImage != null)
            {
                var inverseImageTransform = _imageTransform.Inverse;
                if (inverseImageTransform != null)
                {
                    var centerX = (_endX - _startX) / 2 + _startX;
                    var centerY = (_endY - _startY) / 2 + _startY;
                    var restrictedMinLength = MinCroppedPixelLength * _imageTransform.ScaleX;
                    var maxSelectedLength = Math.Max(_endX - _startX, _endY - _startY);
                    var viewRect = new Rect(centerX - maxSelectedLength / 2, centerY - maxSelectedLength / 2, maxSelectedLength, maxSelectedLength);
                    var uniformSelectedRect = viewRect.GetUniformRect(UsedAspectRatio);
                    if (uniformSelectedRect.Width > _restrictedSelectRect.Width || uniformSelectedRect.Height > _restrictedSelectRect.Height)
                    {
                        uniformSelectedRect = _restrictedSelectRect.GetUniformRect(UsedAspectRatio);
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
                    _currentCroppedRect = inverseImageTransform.TransformBounds(uniformSelectedRect);
                    UpdateImageLayout();
                }
            }
        }

        /// <summary>
        /// Update the visibility of the control button.
        /// </summary>
        private void UpdateControlButtonVisibility()
        {
            var cornerBtnVisibility = CircularCrop ? Visibility.Collapsed : Visibility.Visible;
            var otherBtnVisibility = (CircularCrop || IsSecondaryControlButtonVisible)
                ? Visibility.Visible
                : Visibility.Collapsed;
            if (SourceImage == null)
                cornerBtnVisibility = otherBtnVisibility = Visibility.Collapsed;

            if (_topButton != null)
                _topButton.Visibility = otherBtnVisibility;
            if (_bottomButton != null)
                _bottomButton.Visibility = otherBtnVisibility;
            if (_leftButton != null)
                _leftButton.Visibility = otherBtnVisibility;
            if (_rigthButton != null)
                _rigthButton.Visibility = otherBtnVisibility;
            if (_upperLeftButton != null)
                _upperLeftButton.Visibility = cornerBtnVisibility;
            if (_upperRightButton != null)
                _upperRightButton.Visibility = cornerBtnVisibility;
            if (_lowerLeftButton != null)
                _lowerLeftButton.Visibility = cornerBtnVisibility;
            if (_lowerRigthButton != null)
                _lowerRigthButton.Visibility = cornerBtnVisibility;
        }
    }
}
