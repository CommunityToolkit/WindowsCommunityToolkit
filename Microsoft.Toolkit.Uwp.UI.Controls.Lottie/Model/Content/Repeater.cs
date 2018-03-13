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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Animatable;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content
{
    internal class Repeater : IContentModel
    {
        public Repeater(string name, AnimatableFloatValue copies, AnimatableFloatValue offset, AnimatableTransform transform)
        {
            Name = name;
            Copies = copies;
            Offset = offset;
            Transform = transform;
        }

        internal virtual string Name { get; }

        internal virtual AnimatableFloatValue Copies { get; }

        internal virtual AnimatableFloatValue Offset { get; }

        internal virtual AnimatableTransform Transform { get; }

        public IContent ToContent(LottieDrawable drawable, BaseLayer layer)
        {
            return new RepeaterContent(drawable, layer, this);
        }
    }
}
