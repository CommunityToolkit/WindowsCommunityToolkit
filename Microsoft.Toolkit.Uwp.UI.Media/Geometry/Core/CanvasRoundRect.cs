// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry.Core
{
    /// <summary>
    /// Structure which encapsulates the details of each of the core points
    /// of the path of the rounded rectangle which is calculated based on
    /// either the given (Size, CornerRadius, BorderThickness and Padding)
    /// or (Size, RadiusX and RadiusY).
    /// </summary>
    internal struct CanvasRoundRect
    {
        private const float Factor = 0.5f;

        private readonly float _leftTopWidth;
        private readonly float _topLeftHeight;
        private readonly float _topRightHeight;
        private readonly float _rightTopWidth;
        private readonly float _rightBottomWidth;
        private readonly float _bottomRightHeight;
        private readonly float _bottomLeftHeight;
        private readonly float _leftBottomWidth;

        // This is the location of the properties within the Rect
        //   |--LeftTop----------------------RightTop--|
        //   |                                         |
        // TopLeft                                TopRight
        //   |                                         |
        //   |                                         |
        //   |                                         |
        //   |                                         |
        //   |                                         |
        //   |                                         |
        // BottomLeft                          BottomRight
        //   |                                         |
        //   |--LeftBottom----------------RightBottom--|
        internal Vector2 LeftTop { get; private set; }

        internal Vector2 TopLeft { get; private set; }

        internal Vector2 TopRight { get; private set; }

        internal Vector2 RightTop { get; private set; }

        internal Vector2 RightBottom { get; private set; }

        internal Vector2 BottomRight { get; private set; }

        internal Vector2 BottomLeft { get; private set; }

        internal Vector2 LeftBottom { get; private set; }

        internal Vector2 Size { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRoundRect"/> struct.
        /// </summary>
        /// <param name="origin">Origin of the Rect (absolute location of Top Left corner)</param>
        /// <param name="size">Size of the Rect</param>
        /// <param name="cornerRadius">CornerRadius</param>
        /// <param name="borderThickness">BorderThickness</param>
        /// <param name="padding">Padding</param>
        /// <param name="isOuterBorder">Flag to indicate whether outer or inner border needs
        /// to be calculated</param>
        internal CanvasRoundRect(Vector2 origin, Vector2 size, Vector4 cornerRadius, Vector4 borderThickness, Vector4 padding, bool isOuterBorder)
            : this()
        {
            Size = size;
            var left = Factor * (borderThickness.X + padding.X);
            var top = Factor * (borderThickness.Y + padding.Y);
            var right = Factor * (borderThickness.Z + padding.Z);
            var bottom = Factor * (borderThickness.W + padding.W);

            if (isOuterBorder)
            {
                // Top Left corner radius
                if (cornerRadius.X.IsZero())
                {
                    _leftTopWidth = _topLeftHeight = 0f;
                }
                else
                {
                    _leftTopWidth = cornerRadius.X + left;
                    _topLeftHeight = cornerRadius.X + top;
                }

                // Top Right corner radius
                if (cornerRadius.Y.IsZero())
                {
                    _topRightHeight = _rightTopWidth = 0f;
                }
                else
                {
                    _topRightHeight = cornerRadius.Y + top;
                    _rightTopWidth = cornerRadius.Y + right;
                }

                // Bottom Right corner radius
                if (cornerRadius.Z.IsZero())
                {
                    _rightBottomWidth = _bottomRightHeight = 0f;
                }
                else
                {
                    _rightBottomWidth = cornerRadius.Z + right;
                    _bottomRightHeight = cornerRadius.Z + bottom;
                }

                // Bottom Left corner radius
                if (cornerRadius.W.IsZero())
                {
                    _bottomLeftHeight = _leftBottomWidth = 0f;
                }
                else
                {
                    _bottomLeftHeight = cornerRadius.W + bottom;
                    _leftBottomWidth = cornerRadius.W + left;
                }
            }
            else
            {
                _leftTopWidth = Math.Max(0f, cornerRadius.X - left);
                _topLeftHeight = Math.Max(0f, cornerRadius.X - top);
                _topRightHeight = Math.Max(0f, cornerRadius.Y - top);
                _rightTopWidth = Math.Max(0f, cornerRadius.Y - right);
                _rightBottomWidth = Math.Max(0f, cornerRadius.Z - right);
                _bottomRightHeight = Math.Max(0f, cornerRadius.Z - bottom);
                _bottomLeftHeight = Math.Max(0f, cornerRadius.W - bottom);
                _leftBottomWidth = Math.Max(0f, cornerRadius.W - left);
            }

            // Calculate the anchor points
            ComputeCoordinates(origin);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRoundRect"/> struct.
        /// </summary>
        /// <param name="origin">Top Left corner of the Rounded Rectangle</param>
        /// <param name="size">Dimensions of the Rounded Rectangle</param>
        /// <param name="radiusX">Radius of the corners on the x-axis</param>
        /// <param name="radiusY">Radius of the corners on the y-axis</param>
        internal CanvasRoundRect(Vector2 origin, Vector2 size, float radiusX, float radiusY)
            : this(origin.X, origin.Y, size.X, size.Y, radiusX, radiusY)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRoundRect"/> struct.
        /// </summary>
        /// <param name="x">X offset of the Top Left corner of the Rounded Rectangle</param>
        /// <param name="y">Y offset of the Top Left corner of the Rounded Rectangle</param>
        /// <param name="width">Width of the Rounded Rectangle.</param>
        /// <param name="height">Height of the Rounded Rectangle.</param>
        /// <param name="radiusX">Radius of the corners on the x-axis</param>
        /// <param name="radiusY">Radius of the corners on the y-axis</param>
        internal CanvasRoundRect(float x, float y, float width, float height, float radiusX, float radiusY)
            : this()
        {
            Size = new Vector2(width, height);

            // Sanitize the radii by taking the absolute value
            radiusX = Math.Min(Math.Abs(radiusX), width / 2f);
            radiusY = Math.Min(Math.Abs(radiusY), height / 2);

            _leftTopWidth = radiusX;
            _rightTopWidth = radiusX;
            _rightBottomWidth = radiusX;
            _leftBottomWidth = radiusX;
            _topLeftHeight = radiusY;
            _topRightHeight = radiusY;
            _bottomRightHeight = radiusY;
            _bottomLeftHeight = radiusY;

            ComputeCoordinates(new Vector2(x, y));
        }

        private void ComputeCoordinates(Vector2 origin)
        {
            // compute the coordinates of the key points
            var leftTop = new Vector2(_leftTopWidth, 0);
            var rightTop = new Vector2(Size.X - _rightTopWidth, 0);
            var topRight = new Vector2(Size.X, _topRightHeight);
            var bottomRight = new Vector2(Size.X, Size.Y - _bottomRightHeight);
            var rightBottom = new Vector2(Size.X - _rightBottomWidth, Size.Y);
            var leftBottom = new Vector2(_leftBottomWidth, Size.Y);
            var bottomLeft = new Vector2(0, Size.Y - _bottomLeftHeight);
            var topLeft = new Vector2(0, _topLeftHeight);

            // check anchors for overlap and resolve by partitioning corners according to
            // the percentage of each one.
            // top edge
            if (leftTop.X > rightTop.X)
            {
                var v = _leftTopWidth / (_leftTopWidth + _rightTopWidth) * Size.X;
                leftTop.X = v;
                rightTop.X = v;
            }

            // right edge
            if (topRight.Y > bottomRight.Y)
            {
                var v = _topRightHeight / (_topRightHeight + _bottomRightHeight) * Size.Y;
                topRight.Y = v;
                bottomRight.Y = v;
            }

            // bottom edge
            if (leftBottom.X > rightBottom.X)
            {
                var v = _leftBottomWidth / (_leftBottomWidth + _rightBottomWidth) * Size.X;
                rightBottom.X = v;
                leftBottom.X = v;
            }

            // left edge
            if (topLeft.Y > bottomLeft.Y)
            {
                var v = _topLeftHeight / (_topLeftHeight + _bottomLeftHeight) * Size.Y;
                bottomLeft.Y = v;
                topLeft.Y = v;
            }

            // Apply origin translation
            LeftTop = leftTop + origin;
            RightTop = rightTop + origin;
            TopRight = topRight + origin;
            BottomRight = bottomRight + origin;
            RightBottom = rightBottom + origin;
            LeftBottom = leftBottom + origin;
            BottomLeft = bottomLeft + origin;
            TopLeft = topLeft + origin;
        }
    }
}
