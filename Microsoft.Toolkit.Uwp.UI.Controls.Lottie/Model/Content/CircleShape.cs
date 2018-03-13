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

using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content
{
    internal class CircleShape : IContentModel
    {
        internal CircleShape(string name, IAnimatableValue<Vector2?, Vector2?> position, AnimatablePointValue size, bool isReversed)
        {
            Name = name;
            Position = position;
            Size = size;
            IsReversed = isReversed;
        }

        internal string Name { get; }

        public IAnimatableValue<Vector2?, Vector2?> Position { get; }

        public AnimatablePointValue Size { get; }

        public bool IsReversed { get; }

        public IContent ToContent(LottieDrawable drawable, BaseLayer layer)
        {
            return new EllipseContent(drawable, layer, this);
        }
    }
}