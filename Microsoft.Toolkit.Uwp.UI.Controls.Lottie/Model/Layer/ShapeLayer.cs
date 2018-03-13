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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer
{
    internal class ShapeLayer : BaseLayer
    {
        private readonly ContentGroup _contentGroup;

        internal ShapeLayer(LottieDrawable lottieDrawable, Layer layerModel)
            : base(lottieDrawable, layerModel)
        {
            // Naming this __container allows it to be ignored in KeyPath matching.
            ShapeGroup shapeGroup = new ShapeGroup("__container", layerModel.Shapes);
            _contentGroup = new ContentGroup(lottieDrawable, this, shapeGroup);
            _contentGroup.SetContents(new List<IContent>(), new List<IContent>());
        }

        public override void DrawLayer(BitmapCanvas canvas, Matrix3X3 parentMatrix, byte parentAlpha)
        {
            _contentGroup.Draw(canvas, parentMatrix, parentAlpha);
        }

        public override void GetBounds(out Rect outBounds, Matrix3X3 parentMatrix)
        {
            base.GetBounds(out outBounds, parentMatrix);
            _contentGroup.GetBounds(out outBounds, BoundsMatrix);
        }

        internal override void ResolveChildKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath)
        {
            _contentGroup.ResolveKeyPath(keyPath, depth, accumulator, currentPartialKeyPath);
        }
    }
}