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
    internal class ShapeTrimPath : IContentModel
    {
        public enum Type
        {
            Simultaneously = 1,
            Individually = 2
        }

        private readonly Type _type;
        private readonly AnimatableFloatValue _start;
        private readonly AnimatableFloatValue _end;
        private readonly AnimatableFloatValue _offset;

        internal ShapeTrimPath(string name, Type type, AnimatableFloatValue start, AnimatableFloatValue end, AnimatableFloatValue offset)
        {
            Name = name;
            _type = type;
            _start = start;
            _end = end;
            _offset = offset;
        }

        internal virtual string Name { get; }

        internal new virtual Type GetType()
        {
            return _type;
        }

        internal virtual AnimatableFloatValue End => _end;

        internal virtual AnimatableFloatValue Start => _start;

        internal virtual AnimatableFloatValue Offset => _offset;

        public IContent ToContent(LottieDrawable drawable, BaseLayer layer)
        {
            return new TrimPathContent(layer, this);
        }

        public override string ToString()
        {
            return "Trim Path: {start: " + _start + ", end: " + _end + ", offset: " + _offset + "}";
        }
    }
}